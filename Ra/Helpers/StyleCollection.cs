/*
 * Ra Ajax - An Ajax Library for Mono ++
 * Copyright 2008 - Thomas Hansen polterguy@gmail.com
 * This code is licensed under an MIT(ish) kind of license which 
 * can be found in the license.txt file on disc in addition to that 
 * the code also is licensed under a pure GPL license for those that
 * cannot for some reasons obey by rules in the MIT(ish) kind of license.
 * 
 */

using System;
using System.Collections.Generic;

namespace Ra.Widgets
{
    public class StyleCollection
    {
        private RaWebControl _control;

        public StyleCollection(RaWebControl control)
        {
            _control = control;
        }

        public string this[string idx]
        {
            get
            {
                // Easy validation
                if (idx.ToLower() != idx)
                    throw new ApplicationException("Cannot have a style property which contains uppercase letters");

                // Transforming from CSS file syntax to DOM syntax, e.g. background-color ==> backgroundColor
                // and border-style ==> borderStyle
                while (idx.IndexOf('-') != -1)
                {
                    int where = idx.IndexOf('-');
                    idx = idx.Substring(0, where) + idx.Substring(where + 1, 1).ToUpper() + idx.Substring(where + 2);
                }

                Dictionary<string, string> styles = _control.GetJSONValueDictionary("AddStyle");
                if (styles.ContainsKey(idx))
                    return styles[idx];
                return null;
            }
            set
            {
                // Easy validation
                if (idx.ToLower() != idx)
                    throw new ApplicationException("Cannot have a style property which contains uppercase letters");

                // Transforming from CSS file syntax to DOM syntax, e.g. background-color ==> backgroundColor
                // and border-style ==> borderStyle
                while (idx.IndexOf('-') != -1)
                {
                    int where = idx.IndexOf('-');
                    idx = idx.Substring(0, where) + idx.Substring(where + 1, 1).ToUpper() + idx.Substring(where + 2);
                }

                Dictionary<string, string> styles = _control.GetJSONValueDictionary("AddStyle");
                styles[idx] = value;
            }
        }
    }
}