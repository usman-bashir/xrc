﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Modules
{
    public interface IModuleFactory
    {
        object Get(ComponentDefinition component, IContext context);
		void Release(object component);
    }
}
