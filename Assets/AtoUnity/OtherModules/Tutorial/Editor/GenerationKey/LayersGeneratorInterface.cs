using AtoGame.OtherModules.Assets.AtoUnity.OtherModules.Tutorial.Editor.GenerationKey;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;

/// <summary>
/// The partial keywords tells the compiler to combine this class and Layers
/// Generator into when the project gets compiled. 
/// </summary>
public partial class LayersGeneratorInterface : TutorialKeyGenerator
{
    [MenuItem("Tools/Generate Layers")]
    public static void GenerateLayers()
    {
        //Pop-up a file explorer and ask the client to click where to save the project.
        string outputPath = EditorUtility.SaveFilePanelInProject(
                                 title: "Save Location",
                                 defaultName: "Layers",
                                 extension: "cs",
                                 message: "Where do you want to save this script?");

        //Create a new instance of our generator. 
        TutorialKeyGenerator generator = new TutorialKeyGenerator();

        //Create our session. 
        generator.Session = new Dictionary<string, object>();

        //Get the class name
        string className = Path.GetFileNameWithoutExtension(outputPath);

        //Save it to our session. 
        generator.Session["m_ClassName"] = className;

        //Grab all our layers from Unity. 
        List<string> layers = new List<string>(InternalEditorUtility.layers);

        for (int i = layers.Count - 1; i >= 0; i--)
        {
            //Remove empty entires
            if (string.IsNullOrEmpty(layers[i]))
            {
                layers.RemoveAt(i);
                continue;
            }

            //Remove spaces
            if (layers[i].Contains(" "))
            {
                layers[i] = layers[i].Replace(' ', '_');
            }
        }

        //Add our layers to our generator. 
        generator.Session["m_UnityLayers"] = layers.ToArray();

        //Initialize the template (loads the values from the session into the template)
        generator.Initialize();

        //Generate the definition
        string classDef = generator.TransformText();

        //Write the class to disk
        File.WriteAllText(outputPath, classDef);

        //Tell Unity to refresh. 
        AssetDatabase.Refresh();
    }

}
