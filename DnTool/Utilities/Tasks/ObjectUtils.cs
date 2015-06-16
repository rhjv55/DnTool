#region License
/* 
 * Copyright 2009- Marko Lahma
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not 
 * use this file except in compliance with the License. You may obtain a copy 
 * of the License at 
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0 
 *   
 * Unless required by applicable law or agreed to in writing, software 
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT 
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations 
 * under the License.
 * 
 */
#endregion

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Utilities.Tasks
{
	public static class ObjectUtils
	{

	
		public static T InstantiateType<T>(Type type,TaskContext context)
		{
            if (type == null)
            {
                throw new ArgumentNullException("type", "Cannot instantiate null");
            }
            Type[] types = new Type[1];
            types[0] = typeof(TaskContext);
            ConstructorInfo ci = type.GetConstructor(types);
			if (ci == null)
			{
                throw new ArgumentException("Cannot instantiate type which has no empty constructor", type.Name);
			}
            object[] param = new object[1];
            param[0] = context;
			return (T) ci.Invoke(param);
		}

	
	}

}
