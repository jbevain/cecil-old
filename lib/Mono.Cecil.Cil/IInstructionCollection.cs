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
 * Sat Feb 12 06:06:34 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Cil {

    using System.Collections;

    public interface IInstructionCollection : ICollection, ICodeVisitable {

        IInstruction this [int index] { get; set; }
        
        void Add (IInstruction value);
        void Clear ();
        bool Contains (IInstruction value);
        int IndexOf (IInstruction value);
        void Insert (int index, IInstruction value);
        void Remove (IInstruction value);
        void RemoveAt (int index);
    }
}