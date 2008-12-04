﻿/*
 * Ra-Ajax - A Managed Ajax Library for ASP.NET and Mono
 * Copyright 2008 - Thomas Hansen thomas@ra-ajax.org
 * This code is licensed under the LGPL version 3 which 
 * can be found in the license.txt file on disc.
 * 
 */

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace RaSelector
{
    /**
     * Helper class for doing selector queries on Controls to retrieve specific 
     * Controls in Page hierarchy. Kind of like a CSS selector only for the server-side.
     * This class can be very handsome in scenarios where you have GridViews or Data Repeators
     * which are dynamically databound and you for some reason cannot use the ID directly or something
     * but still need to traverse server-side Control hierarchy to find a specific control...
     */
    public static class Selector
    {
        /**
         * Recursively searches Control hierarchy for matches matching FindDelegate and returns it as T
         */
        public static T SelectFirst<T>(Control from, Predicate<Control> predicate) where T : Control
        {
            if (predicate(from))
                return from as T;
            foreach (Control idx in from.Controls)
            {
                T tmpRetVal = SelectFirst<T>(idx, predicate);
                if (tmpRetVal != null)
                    return tmpRetVal;
            }
            return null;
        }

        /**
         * Recursively searches Control hierarchy for the first control that 
         * matches the type of T and returns it as T
         */
        public static T SelectFirst<T>(Control from) where T : Control
        {
            return SelectFirst<T>(from,
                delegate(Control idx)
                {
                    return idx is T;
                });
        }

        /**
         * Recursively search Control hierarcy for ALL controls that 
         * matches the given delegate and returns them as T
         */
        public static IEnumerable<T> Select<T>(Control from, Predicate<Control> predicate) where T : Control
        {
            if (from is T)
            {
                if (predicate(from))
                    yield return from as T;
            }
            foreach (Control idx in from.Controls)
            {
                foreach (T idxInner in Select<T>(idx))
                {
                    yield return idxInner;
                }
            }
        }

        /**
         * Recursively search Control hierarcy for ALL controls that 
         * matches the type of T and returns them as T
         */
        public static IEnumerable<T> Select<T>(Control from) where T : Control
        {
            return Select<T>(from,
                delegate(Control idx)
                {
                    return idx is T;
                });
        }
    }
}
