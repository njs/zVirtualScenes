﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zvs.Entities
{
    [Table("QueuedCommands", Schema = "ZVS")]
    public class QueuedCommand : INotifyPropertyChanged
    {
        public int QueuedCommandId { get; set; }

        public virtual Command Command { get; set; }

        private string _Argument;
        [StringLength(512)]
        public string Argument
        {
            get
            {
                return _Argument;
            }
            set
            {
                if (value != _Argument)
                {
                    _Argument = value;
                    NotifyPropertyChanged("Argument");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public static event NewCommandEventHandler NewCommandCommand;
        public delegate void NewCommandEventHandler(object sender, NewCommandArgs args);

        public static void AddNewCommandCommand(NewCommandArgs args)
        {
            if (NewCommandCommand != null)
                NewCommandCommand(null, args);
        }

        public class NewCommandArgs
        {
            public NewCommandArgs(QueuedCommand command)
            {
                Command = command;
            }

            public QueuedCommand Command { private set; get; }
        }
    }
}