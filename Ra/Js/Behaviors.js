/*
 * Ra-Ajax - A Managed Ajax Library for ASP.NET and Mono
 * Copyright 2008 - 2009 - Thomas Hansen thomas@ra-ajax.org
 * This code is licensed under the LGPL version 3 which 
 * can be found in the license.txt file on disc.
 * 
 */


// ==============================================================================
//
// All the default Ajax Behaviors which exists in Ra-Ajax core project
// First the Base class for all behaviors.
// Note that the Behaviors have "delayed construction" which actually 
// is fired AFTER the Ra.Control is finished initializing which is why
// you don't see a forward call from the Ra.Beha.init to the
// specific overridden initBehavior which does the actual "initializsation"
// of the Behavior.
//
// ==============================================================================


(function(){

Ra.Beha = Ra.klass();


// Retrieve function
Ra.Beha.$ = function(id) {
  var ctrls = Ra.Control._controls;
  var idxCtrl = ctrls.length;
  while( idxCtrl-- ) {
    var behvs = ctrls[idxCtrl].options.beha;
    var idxBeha = behvs.length;
    while( idxBeha-- ) {
      if( behvs[idxBeha].id == id ) {
        return behvs[idxBeha];
      }
    }
  }
};


Ra.Beha.prototype = {

  // CTOR
  init: function(id, opt) {
    this.options = opt;
    this.id = id;
  },

  initBehavior: function() {
  },

  handleJSON: function(json) {
    // Looping through all "top-level" objects and calling the functions for those keys
    for( var idxKey in json ) {
      this[idxKey](json[idxKey]);
    }
  }
};









// ==============================================================================
//
// This is the BehaviorDraggable
// By using this you can get a control which can be dragged and dropped
// on the screen which again will trigger a server-side event
// which you can trap in your own code.
// It has properties like "snap" which makes it draggable within a "grid"
// and bounds which tells it a rectangular boundary which is the max/min
// boundaries of the draggable area.
//
// ==============================================================================
Ra.BDrag = Ra.klass();


// Inheriting from Behavior base class
Ra.extend(Ra.BDrag.prototype, Ra.Beha.prototype);


// We need to extend the document.body since were using 
// the observe function later down the road here on the 
// document.body element
Ra.extend(document.body, Ra.Element.prototype);


// Creating IMPLEMENTATION of class
Ra.extend(Ra.BDrag.prototype, {

  // Delayed CTOR, actually called by the Ra.Control class
  // for all Behaviors within the Control
  initBehavior: function(parent) {

    this._hasCaption = false;
    this.parent = parent;
    
    this.options = Ra.extend({
      bounds: {left:-2000, top:-2000, width: 4000, height: 4000},
      snap:{x:1,y:1},
      handle: this.parent.element
    }, this.options || {});

    // To prevent selection for non-mozilla browsers
    this.options.handle.observe('mousedown', this.onMouseDown, this);
    document.body.observe('mouseup', this.onMouseUp, this);
    document.body.observe('mousemove', this.onMouseMove, this);

  },

  // Setter for the Bounds Rectangle which determines the max/min
  // position the control can be dragged around within.
  Bounds: function(rc) {
    this.options.bounds = rc;
  },

  Handle: function(handle) {
    this.options.handle.stopObserving('mousedown', this.onMouseDown, this);
    this.options.handle = Ra.$(handle);
    handle.observe('mousedown', this.onMouseDown, this);
  },

  // Setter for the Snap Point which determines how the 
  // control is supposed to "snap" when dragged
  Snap: function(pt) {
    this.options.snap = pt;
  },

  onStopEvent: function(evt) {
    return false;
  },

  // Called when mouse is being pushed DOWN on top of the Control
  onMouseDown: function(event) {
    this._hasCaption = true;
    this._hasDragged = false;
    this._pos = this.pointer(event);
    
    this.parent.element.absolutize();
    
    // Storing old position
    this._oldX = parseInt(this.parent.element.getStyle('left'), 10);
    this._oldY = parseInt(this.parent.element.getStyle('top'), 10);

    // Stopping "selection mode"
    document.body.observe('selectstart', this.onStopEvent);
    document.body.observe('dragstart', this.onStopEvent);
  },

  // Called when mouse is released. Note that this
  // is currently being trapped for the DOM element of the control
  // but should be trapped for the document.body element.
  onMouseUp: function(event) {
    if( !this._hasCaption || !this._hasDragged ) {
      return;
    }
    this._hasCaption = false;
    delete this._pos;

    // Calling server to update new position of element and potentially raise event
    var newX = parseInt(this.parent.element.getStyle('left'), 10);
    var newY = parseInt(this.parent.element.getStyle('top'), 10);

    // Calling server with new position and (maybe) raising the Dropped event
    var mouse = this.pointer(event);
    var affectedDroppers = Ra.BDrop.getAffected(mouse.x, mouse.y);
    var dropParams = '';
    if( affectedDroppers.length > 0 ) {
      var idx = affectedDroppers.length;
      dropParams = '&drops=';
      while( idx-- ) {
        if( idx > 0 ) {
          dropParams += ',';
        }
        dropParams += affectedDroppers[idx].id;
        affectedDroppers[idx].unTouch();
      }
    }
    var dummy = new Ra.Ajax({
      args:'__RA_CONTROL=' + this.id + '&__EVENT_NAME=dropped' + '&x=' + newX + '&y=' + newY + dropParams,
      raCallback:true,
      onSuccess: this.onFinishedRequest,
      onError: this.onFailedRequest,
      callingContext: this
    });
    // Stopping "selection mode"
    document.body.stopObserving('selectstart', this.onStopEvent);
    document.body.stopObserving('dragstart', this.onStopEvent);
  },

  // Called when mouse is moved. This too have the same "bug" as the
  // function above.
  onMouseMove: function(event) {
    if( this._hasCaption ) {
      this._hasDragged = true;
      var pos = this.pointer(event);
      var xDelta = pos.x - this._pos.x;
      var yDelta = pos.y - this._pos.y;
      xDelta -= xDelta % this.options.snap.x;
      yDelta -= yDelta % this.options.snap.y;
      var bn = this.options.bounds;
      this.parent.element.setStyle('left',Math.min(Math.max(this._oldX + xDelta, bn.left), bn.width + bn.left) + 'px');
      this.parent.element.setStyle('top',Math.min(Math.max(this._oldY + yDelta, bn.top), bn.height + bn.top) + 'px');

      // Signaling affected droppers
      var affectedDroppers = Ra.BDrop.getAffected(pos.x, pos.y);
      var idx = affectedDroppers.length;
      while( idx-- ) {
        affectedDroppers[idx].touched();
      }
      // Unsignalizing UN-affected ones
      affectedDroppers = Ra.BDrop.getUnAffected(pos.x, pos.y);
      idx = affectedDroppers.length;
      while( idx-- ) {
        affectedDroppers[idx].unTouch();
      }
    }
  },

  onFinishedRequest: function(response) {
    eval(response);
  },

  onFailedRequest: function(status, fullTrace) {
    Ra.Control.errorHandler(status, fullTrace);
  },

  // Returns the x,y position of the mouse from the given event
  pointer: function(event) {
    return {
      x: event.pageX || (event.clientX +
        (document.documentElement.scrollLeft || document.body.scrollLeft)),
      y: event.pageY || (event.clientY +
        (document.documentElement.scrollTop || document.body.scrollTop))
    };
  },

  // Called when Control is being destroyed
  destroy: function() {
    this.options.handle.stopObserving('mousedown', this.onMouseDown, this);
    this.options.handle.stopObserving('mousedown', this.onMouseDown, this);
    document.body.stopObserving('mouseup', this.onMouseUp, this);
    document.body.stopObserving('mousemove', this.onMouseMove, this);
  }
});






// ==============================================================================
//
// This is the BehaviorDroppable
// A Droppable is an element which can handle draggables dropped on
// top of it.
// Inspired from ScriptAculous and Prototype.js
//
// ==============================================================================
Ra.BDrop = Ra.klass();


// Registered droppers on page
Ra.BDrop._droppers = [];


Ra.BDrop.getAffected = function(x, y) {
  var retVal = [];
  var drps = Ra.BDrop._droppers;
  var idx = drps.length;
  while( idx-- ) {
    if( drps[idx].parent.element.within(x,y) ) {
      retVal.push(drps[idx]);
    }
  }
  return retVal;
};


Ra.BDrop.getUnAffected = function(x, y) {
  var retVal = [];
  var drps = Ra.BDrop._droppers;
  var idx = drps.length;
  while( idx-- ) {
    if( !drps[idx].parent.element.within(x,y) ) {
      retVal.push(drps[idx]);
    }
  }
  return retVal;
};


// Inheriting from Ra.Control
Ra.extend(Ra.BDrop.prototype, Ra.Beha.prototype);


// Creating IMPLEMENTATION of class
Ra.extend(Ra.BDrop.prototype, {

  // Delayed CTOR, actually called by the Ra.Control class
  // for all Behaviors within the Control
  initBehavior: function(parent) {
    this.parent = parent;
    Ra.BDrop._droppers.push(this);

    this.options = Ra.extend({
      touched: null
    }, this.options || {});
  },

  TouchedCssClass: function(value) {
    this.options.touched = value;
  },

  touched: function() {
    if( this.isTouched || this.parent.element.className.indexOf(this.options.touched) != -1 ) {
      return;
    }
    this.isTouched = true;
    this._oldClassName = this.parent.element.className;
    this.parent.element.addClassName(this.options.touched);
  },

  unTouch: function() {
    if( !this.isTouched ) {
      return;
    }
    this.isTouched = false;
    this.parent.element.removeClassName(this.options.touched);
  },

  destroy: function() {
    var idx = Ra.BDrop._droppers.length;
    while( idx-- ) {
      if( Ra.BDrop._droppers[idx].id == this.id ) {
        // We have found our instance, idxToRemove now should contain the index of the control
        break;
      }
    }

    // Removes control out from registered controls collection
    Ra.Control._controls.splice(idx, 1);
  }
});

})();