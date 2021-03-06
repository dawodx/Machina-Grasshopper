﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using GH_IO.Serialization;

using Machina;
using MachinaGrasshopper.GH_Utils;

namespace MachinaGrasshopper.Actions
{
    //  ███████╗██╗  ██╗████████╗███████╗██████╗ ███╗   ██╗ █████╗ ██╗      █████╗ ██╗  ██╗███████╗███████╗
    //  ██╔════╝╚██╗██╔╝╚══██╔══╝██╔════╝██╔══██╗████╗  ██║██╔══██╗██║     ██╔══██╗╚██╗██╔╝██╔════╝██╔════╝
    //  █████╗   ╚███╔╝    ██║   █████╗  ██████╔╝██╔██╗ ██║███████║██║     ███████║ ╚███╔╝ █████╗  ███████╗
    //  ██╔══╝   ██╔██╗    ██║   ██╔══╝  ██╔══██╗██║╚██╗██║██╔══██║██║     ██╔══██║ ██╔██╗ ██╔══╝  ╚════██║
    //  ███████╗██╔╝ ██╗   ██║   ███████╗██║  ██║██║ ╚████║██║  ██║███████╗██║  ██║██╔╝ ██╗███████╗███████║
    //  ╚══════╝╚═╝  ╚═╝   ╚═╝   ╚══════╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝╚══════╝
    //                                                                                                     
    public class ExternalAxis : GH_MutableComponent
    {
        public ExternalAxis() : base(
            "ExternalAxis",
            "ExternalAxis",
            "Increase the value of one of the robot's external axis. Values expressed in degrees or milimeters, depending on the nature of the external axis. Note that the effect of this change of external axis will go in effect on the next motion Action.",
            "Machina",
            "Actions")
        { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override Guid ComponentGuid => new Guid("b304ce32-0ab1-4d8e-b20a-857b6fc1a55e");
        protected override System.Drawing.Bitmap Icon => null;

        protected override bool ShallowInputMutation => true;

        protected override void RegisterMutableInputParams(GH_MutableInputParamManager mpManager)
        {
            // Absolute
            mpManager.AddComponentNames(false, "ExternalAxisTo", "ExternalAxisTo", "Set the values of one of the robot's external axes.");
            mpManager.AddParameter(false, typeof(Param_Integer), "AxisNumber", "EAid", "Axis number from 1 to 6.", GH_ParamAccess.item);
            mpManager.AddParameter(false, typeof(Param_Number), "Value", "v", "Increment value in mm or degrees.", GH_ParamAccess.item);

            // Relative
            mpManager.AddComponentNames(true, "ExternalAxis", "ExternalAxis", "Increase the values of one of the robot's external axes.");
            mpManager.AddParameter(true, typeof(Param_Integer), "AxisNumber", "EAid", "Axis number from 1 to 6.", GH_ParamAccess.item);
            mpManager.AddParameter(true, typeof(Param_Number), "Increment", "inc", "New value in mm or degrees.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Action", "A", "ExternalAxis Action", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int axisNumber = 1;
            double val = 0;

            if (!DA.GetData(0, ref axisNumber)) return;
            if (!DA.GetData(1, ref val)) return;

            if (axisNumber < 1 || axisNumber > 6)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "AxisNumber must be between 1 and 6");
            }

            DA.SetData(0, new ActionExternalAxis(axisNumber, val, this.Relative));
        }
    }
}
