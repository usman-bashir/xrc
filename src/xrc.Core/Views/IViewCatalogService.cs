﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xrc.Views
{
    public interface IViewCatalogService
    {
        IEnumerable<ComponentDefinition> GetAll();
        ComponentDefinition Get(string name);
    }
}
