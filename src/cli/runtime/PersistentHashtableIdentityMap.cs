/**
 *   Copyright (c) Rich Hickey. All rights reserved.
 *   The use and distribution terms for this software are covered by the
 *   Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
 *   which can be found in the file CPL.TXT at the root of this distribution.
 *   By using this software in any fashion, you are agreeing to be bound by
 * 	 the terms of this license.
 *   You must not remove this notice, or any other, from this software.
 **/

using System;
using System.Collections;

namespace org.clojure.runtime
	{
	
public class PersistentHashtableIdentityMap : PersistentHashtableMap {

public PersistentHashtableIdentityMap(int initialCapacity) : base(initialCapacity) {
}

public PersistentHashtableIdentityMap(Object[] init) : base(init){
}

PersistentHashtableIdentityMap(int count, PersistentArray array) : base(count, array) {
}


override public IEnumerator GetEnumerator()
	{
	return new Iter2(array);
	}


internal class Iter2 : IEnumerator
	{
	PersistentArray buckets;
	int b;
	Object e;

	internal Iter2(PersistentArray buckets)
		{
		this.buckets = buckets;
		this.b = -1;
		}

	private void nextBucket()
		{
		e = null;
		for (b = b + 1; b < buckets.length(); b++)
			{
			Object a = buckets.get(b);
			if (a != null && a != PersistentListIdentityMap.EMPTY)
				{
				e = a;
				break;
				}
			}
		}

	#region IEnumerator Members

	public object Current
		{
		get { return e; }
		}

	public bool MoveNext()
		{
		if (e == null || (e = ((PersistentListIdentityMap)e).rest()) == PersistentListIdentityMap.EMPTY)
			nextBucket();
		return e != null;
		}

	public void Reset()
		{
		throw new Exception("The method or operation is not implemented.");
		}

	#endregion
	}


IPersistentMap create(int capacity) {
    return new PersistentHashtableIdentityMap(capacity);
}

IPersistentMap create(int count, PersistentArray array) {
    return new PersistentHashtableIdentityMap(count, array);
}

IPersistentMap createListMap(Object key, Object val){
    return PersistentListIdentityMap.create(key,val);
}

}

}
