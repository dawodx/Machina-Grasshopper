﻿using System;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Machina;

namespace MachinaGrasshopper.Tools
{
    //  ████████╗ ██████╗  ██████╗ ██╗        ███╗   ██╗███████╗██╗    ██╗
    //  ╚══██╔══╝██╔═══██╗██╔═══██╗██║        ████╗  ██║██╔════╝██║    ██║
    //     ██║   ██║   ██║██║   ██║██║        ██╔██╗ ██║█████╗  ██║ █╗ ██║
    //     ██║   ██║   ██║██║   ██║██║        ██║╚██╗██║██╔══╝  ██║███╗██║
    //     ██║   ╚██████╔╝╚██████╔╝███████╗██╗██║ ╚████║███████╗╚███╔███╔╝
    //     ╚═╝    ╚═════╝  ╚═════╝ ╚══════╝╚═╝╚═╝  ╚═══╝╚══════╝ ╚══╝╚══╝ 
    //                                                                    
    public class ToolCreate : GH_Component
    {
        public ToolCreate() : base(
            "Tool.Create",
            "Tool.Create",
            "Create a new Tool object.", 
            "Machina", 
            "Tools") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override Guid ComponentGuid => new Guid("19e1c38a-94f8-41b6-b5a5-0a549fdf0123");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Tools_Create;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "T", "Tool name", GH_ParamAccess.item, "ToolExMachina");
            pManager.AddPlaneParameter("BasePlane", "BP", "Base Plane where the Tool will be attached to the Robot", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddPlaneParameter("TCPPlane", "TP", "Plane of the Tool Tip Center (TCP)", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddNumberParameter("Weight", "W", "Tool weight in Kg", GH_ParamAccess.item, 1);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Tool", "T", "New Tool object", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = "";
            Plane bpl = Plane.WorldXY;
            Plane tcppl = Plane.WorldXY;
            double w = 0;

            if (!DA.GetData(0, ref name)) return;
            if (!DA.GetData(1, ref bpl)) return;
            if (!DA.GetData(2, ref tcppl)) return;
            if (!DA.GetData(3, ref w)) return;

            // Create a TCP plane as 
            Rhino.Geometry.Transform rel = Rhino.Geometry.Transform.ChangeBasis(Plane.WorldXY, bpl);
            if (!tcppl.Transform(rel))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid input planes");
                return;
            }

            Point3d cog = 0.5 * tcppl.Origin;
            Tool tool = Tool.Create(name,
                tcppl.OriginX, tcppl.OriginY, tcppl.OriginZ,
                tcppl.XAxis.X, tcppl.XAxis.Y, tcppl.XAxis.Z, tcppl.YAxis.X, tcppl.YAxis.Y, tcppl.YAxis.Z,
                w,
                cog.X, cog.Y, cog.Z);

            DA.SetData(0, tool);
        }
    }

}
