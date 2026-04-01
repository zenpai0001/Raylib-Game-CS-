using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Raylib_Game_CS_
{
        public interface ISerializable
        {
            string jsonFilePath { get; set; }

            string jsonContent { get; set;  }
            public void Deserialize(ISerializable serializable)
            {
                jsonContent = File.ReadAllText(jsonFilePath);
            }
            public void Serialize()
            {

            }
        }
    }
