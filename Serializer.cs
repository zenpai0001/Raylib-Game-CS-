using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Raylib_Game_CS_
{
        public interface ISerializable <T>
        {
            string jsonFilePath { get; set; }

            string jsonContent { get; set;  }
            public virtual void Deserialize(ISerializable<T> serializable)
            {
                jsonContent = File.ReadAllText(jsonFilePath);
            }
            public virtual void Serialize()
            {

            }
        }
    }
