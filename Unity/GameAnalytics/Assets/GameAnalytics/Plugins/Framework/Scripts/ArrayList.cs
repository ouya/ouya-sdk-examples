#if UNITY_METRO
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace GA_Compatibility.Collections
{
    public class ArrayList : List<Object>
    {
		
		public ArrayList ()
		{
		
		}
		
		
		public ArrayList (ICollection c)
		{
			if (c == null) {
				throw new ArgumentNullException ("c", "ArgumentNull_Collection");
			}
			foreach(object item in c) {
    			this.Add(item);
			}
		}
		
    }
}
#endif