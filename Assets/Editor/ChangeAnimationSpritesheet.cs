using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace Assets.Editor
{
    public static class ChangeAnimationSpritesheet
    {
        // This is a hack but meh find a better solution another day should really just be a runtime spritesheet swapout
        [MenuItem("Buster/Hacks/Perform Spritesheet Animation Swapover")]
        public static void ChangeAnimationOver()
        {
            const string OLD_SPRITESHEET_GUID = "b9b088d3c28f126418d23a9f4e620364";
            const string NEW_SPRITESHEET_GUID = "a132964ae91839647a5d0508d2181254";

            string startFolder = Path.Combine(Application.dataPath, "Animations/Character/Player");
            string targetFolder = Path.Combine(Application.dataPath, "Animations/Character/YellowPlayer");

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            var controllerInfo = (new DirectoryInfo(startFolder)).GetFiles("*.controller")[0];
            var controllerData = File.ReadAllText(controllerInfo.FullName);

            var guidRegex = new Regex("guid: ([0-9a-f]+)");
            foreach (var animFileInfo in (new DirectoryInfo(startFolder).GetFiles("*.anim")))
            {
                string targetFile = Path.Combine(targetFolder, animFileInfo.Name);
                string animFileData = File.ReadAllText(animFileInfo.FullName);
                animFileData = animFileData.Replace(OLD_SPRITESHEET_GUID, NEW_SPRITESHEET_GUID);
                File.WriteAllText(targetFile, animFileData);

                string metaFileText = File.ReadAllText(animFileInfo.FullName + ".meta");

                Match guidMatch = guidRegex.Match(metaFileText);
                string existingGuid = guidMatch.Groups[1].Captures[0].Value;
                string newGuid = GUID.Generate().ToString();
                metaFileText = metaFileText.Replace(existingGuid, newGuid);

                controllerData = controllerData.Replace(existingGuid, newGuid);

                File.WriteAllText(targetFile + ".meta", metaFileText);
            }
            
            var targetControllerFile = Path.Combine(targetFolder, controllerInfo.Name);
            File.WriteAllText(targetControllerFile, controllerData);

            AssetDatabase.Refresh();
        }
    }
}
