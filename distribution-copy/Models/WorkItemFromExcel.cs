﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkItemPublish
{
    class WorkitemFromExcel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        //public string createdID { get; set; }
        public ParentWorkItem Parent { get; set; }
        public string WiState { get; set; }
        public int Old_ID { get; set; }
        public string AreaPath { get; set; }
        public string Itertation { get; set; }
        //public string  WiState { get; set; }
    }
    class ParentWorkItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        //public string createdID { get; set; }
    }
}
