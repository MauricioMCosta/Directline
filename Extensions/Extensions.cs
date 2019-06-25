using Directline.Services;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Directline.Extensions
{
    public static class Extensions
    {
        public static void RunAsCustomService(this IWebHost host)
        {
            var webHostService = new CustomWebHostService(host);
            ServiceBase.Run(webHostService);
        }
        public static void CopyFrom(this object objDestination, object objSource, string[] skipProperties)
        {
            //get the list of all properties in the destination object
            var destProps = objDestination.GetType().GetProperties();

            //get the list of all properties in the source object
            foreach (var sourceProp in objSource.GetType().GetProperties().Where(p=>!skipProperties.Contains(p.Name)))
            {
                
                var dProperty = destProps.SingleOrDefault(p => p.Name.Equals(sourceProp.Name) && p.CanWrite);
                if(dProperty==null) continue;
                if(sourceProp.GetValue(objSource)==null) continue; // skip nulls.
                dProperty.SetValue(objDestination, sourceProp.GetValue(objSource));
            }
        }

    }
}
