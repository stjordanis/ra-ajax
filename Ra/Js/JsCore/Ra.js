/*
 * Ra Ajax, Copyright 2008 - Thomas Hansen
 * All JS methods and objects are inside of the
 * Ra namespace.
 */


// Creating main namespace
Ra = {}


// $ method, used to retrieve elements on document
Ra.$ = function(id) {
  var el = document.getElementById(id);
  Ra.extend(el, Ra.Element.prototype);
  return el;
}


// To create a class which will automatically call "init" on objects 
// when created with the arguments applied
Ra.klass = function() {
  return function(){
    if( this.init )
      return this.init.apply(this, arguments);
  };
}


// Takes one base object and one inherited object where all
// the properties and functions from the base object is copied into the 
// inherited object. The inherited object is returned
// for creating better syntax
Ra.extend = function(inherited, base) {
  for (var prop in base)
    inherited[prop] = base[prop];
  return inherited;
}


// Element class, used as helper to manipulate DOM elements
Ra.Element = Ra.klass();


Ra.extend(Ra.Element.prototype, {
  replace: function(html) {
  }
});











