/*
 * Copyright (c) 2004 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jb.evain@dotnetguru.org)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 * Generated by /CodeGen/cecil-gen.rb do not edit
 * Sun Jan 30 17:29:13 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil {

    using System.Collections;

    public interface IOverrideCollection : ICollection, IReflectionVisitable {

        IMethodReference this [string name] { get; set; }

        IMethodDefinition Container { get; }

        void Clear ();
        bool Contains (IMethodReference value);
        void Remove (IMethodReference value);
    }
}