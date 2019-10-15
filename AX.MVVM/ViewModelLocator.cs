using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace AX.MVVM
{

    public class ViewModelLocator
    {
        private Type[] types;

#if NETSTANDARD2_0 || NETFRAMEWORK
        public ViewModelLocator()
        {
            types = Assembly.GetCallingAssembly().GetTypes();        
        }
#endif
        public ViewModelLocator(IEnumerable<Type> types)
        {
            this.types = types.ToArray();
        }

        public IViewModel<T> LocateViewModel<T>(T model)
        {
            var modelType = model.GetType();
            var vmType = typeof(IViewModel<>).MakeGenericType(modelType);
            var vmTypeInfo = vmType.GetTypeInfo();

            var vmTypes = types.Where(t => t.GetTypeInfo().IsAssignableFrom(vmTypeInfo)).ToArray();

            if (vmTypes.Length > 1)
            {
                throw new ArgumentException("There are multiple ViewModels for this Model");
            }
            else if (vmTypes.Length == 0)
            {
                throw new ArgumentException("There are no ViewModels for this Model");
            }
            else
            {
                return Activator.CreateInstance(vmTypes.First(), model) as IViewModel<T>;
            }
        }

    }

}
