using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xrc;

namespace DemoWebSite
{
    public class DemoKernel : Kernel
    {
        public DemoKernel(string workingPath)
            :base(workingPath)
        {
        }

        protected override void Init()
        {
            base.Init();

            RegisterComponentsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());

            Modules.Add(new Module("Weather", typeof(WeatherModule)));
            Modules.Add(new Module("GoogleNews", typeof(GoogleNewsModule)));
			Modules.Add(new Module("Player", typeof(PlayerModule)));
		}
    }
}