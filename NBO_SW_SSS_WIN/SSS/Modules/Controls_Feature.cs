using System;
using System.Collections.Generic;
//using YamlDotNet.RepresentationModel;


namespace ModuleLayer
{
    public abstract class Feature
    {
        public string Name { get; protected set; }
        public byte Code { get; protected set; }
        public TimeSpan Delay { get; protected set; }
        public string Description { get; protected set; }


        protected Feature(string name, byte code, float delaySeconds = 0.5f)
        {
            Name = name;
            Code = code;
            Delay = TimeSpan.FromSeconds(delaySeconds);
        }

        //public abstract bool TryParseValue(YamlNode node, out uint value);
        public abstract bool TryParseValue(string str, out uint value);

        public abstract string ValueName(uint value);

        //public abstract string YamlConfigTemplate();
    }

    public class FeatureRange : Feature
    {
        public uint From { get; private set; }
        public uint To { get; private set; }

        public FeatureRange(string name, byte code, uint from, uint to, float delaySeconds = 0, string description = null) : base(name, code, delaySeconds)
        {
            From = from;
            To = to;
            Description = description ?? $"{Name}: value beetween {From} and {To}";
        }

        public override bool TryParseValue(string str, out uint value)
        {
            bool res = false;
            if (uint.TryParse(str, out value))
            {
                if (value >= From || value <= To)
                    res = true;
            }

            return res;
        }

        public override string ValueName(uint value)
        {
            return value.ToString();
        }
    }

    public class Feature<T> : Feature
    {
        public Feature(string name, byte code, float delaySeconds = 0) : base(name, code, delaySeconds)
        {
            Description = $"{Name}: {string.Join(", ", Enum.GetNames(typeof(T)))}";
        }

        public override bool TryParseValue(string str, out uint value)
        {
            bool res = false;
            try
            {
                value = (uint)Enum.Parse(typeof(T), str, true);
            }
            catch
            {
                value = 999;
                res = false;
                return res;
            }

            if (Enum.IsDefined(typeof(T), value))
                res = true;

            return res;
        }

        public override string ValueName(uint value)
        {
            if (Enum.IsDefined(typeof(Enum), value))
                return (Enum.ToObject(typeof(Enum), value)).ToString();
            else
                return value.ToString();
        }
    }

    class MonitorFeatures
    {
        public List<Feature> features = new List<Feature>()
        {
            new FeatureRange("Brightness", 0x10, 0, 100, description: "Brightness: value beetween 0 and 100 where 0 is the minimal brightness, 100 is the maximal brightness"),
            new Feature<StandardColor>("StandardColor", 0x14),
            new Feature<AudioInput>("AudioInput", 0x1D, delaySeconds: 4),
            new Feature<LowInputLag>("LowInputLag", 0x23),
            new Feature<ResponceTime>("ResponceTime", 0x25),
            new Feature<VideoInputAutodetect>("VideoInputAutodetect", 0x33, delaySeconds: 4),
            new Feature<VideoInput>("VideoInput", 0x60, delaySeconds: 4),
            new FeatureRange("Volume", 0x62, 0, 100, description: "Volume: value beetween 0 and 100 where 0 is the minimal volume, 100 is the maximal volume"),
            new Feature<AmbientLightSensor>("AmbientLightSensor", 0x66),
            new Feature<PresenceSensor>("PresenceSensor", 0x67),
            new Feature<AudioMute>("AudioMute", 0x8D),
            //new FeaturePIPPosition("PIPPosition", 0x96),
            new FeatureRange("PIPSize", 0x97, 0, 10, description: "PIPSize: value beetween 0 and 10 where 0 is the minimal size, 10 is the maximal size"),
            new Feature<DisplayApplication>("DisplayApplication", 0xDC),
            new Feature<MultiPicture>("MultiPicture", 0xE8, delaySeconds: 4),
            new Feature<Uniformity>("Uniformity", 0xE9),
        };


        //public static bool TryParse(YamlNode keyNode, YamlNode valueNode, out Feature feature, out uint value)
        /*public static bool TryParse(out string enumName, out uint value)
        {
            //if (keyNode.NodeType == YamlNodeType.Scalar)
            //    if ((feature = features.Find(_feature => _feature.Name == ((YamlScalarNode)keyNode).Value)) != null)
                    if (feature.TryParseValue(valueNode, out value))
                        return true;

            feature.Name = enumName;
            value = default;
            return false;
        }

        
        public static string YamlConfigTemplate()
        {
            return string.Join("\r\n\r\n", features.Select(feature => feature.YamlConfigTemplate()));
        }
        */
    }


    public enum StandardColor : uint
    {
        NotAvailable = 0,
        sRGB = 1,
        Adobe = 14,
        EBU = 15,
        SMPTEC = 16,
        REC709 = 17,
        DICOMSIM = 18,
        DCIP3 = 19,
        CAL1 = 21,
        CAL2 = 22,
        CAL3 = 23,
        iPhone = 24,
        Custom = 255,
    }


    public enum AudioInput : uint
    {
        NotAvailable = 0,
        DisplayPort = 15,
        HDMI1 = 17,
        HDMI2 = 18,
        MiniDisplayPort = 21,
        TypeC = 23,
        Auto = 241,
    }


    public enum LowInputLag : uint
    {
        NotAvailable = 0,
        Off = 1,
        Advanced = 2,
        UltraFast = 3,
    }


    public enum ResponceTime : uint
    {
        NotAvailable = 0,
        Standard = 1,
        Advanced = 2,
        UltraFast = 3,
    }


    public enum VideoInputAutodetect : uint
    {
        NotAvailable = 0,
        Off = 1,
        On = 2,
    }


    public enum VideoInput : uint   //@MCCS_2.2a Tbale8-13
    {
        NotAvailable = 0,
        DVI1 = 3,
        DVI2 = 4,
        DisplayPort1 = 15,
        DisplayPort2 = 16,
        HDMI1 = 17,
        HDMI2 = 18,
        //MiniDisplayPort = 21,
        //TypeC = 23,
    }


    public enum AmbientLightSensor : uint
    {
        NotAvailable = 0,
        On = 1,
        Off = 2,
    }


    public enum PresenceSensor : uint
    {
        Off = 0,
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
    }


    public enum AudioMute : uint
    {
        NotAvailable = 0,
        Mute = 1,
        Unmute = 2,
    }


    public enum DisplayApplication : uint
    {
        Off = 0,
        Movie = 3,
        FPS1 = 8,
        FPS2 = 49,
        RTS = 50,
        MODA = 51,
        Web = 52,
        Text = 53,
        MAC = 54,
        CADCAM = 55,
        Animation = 56,
        VideoEdit = 57,
        Retro = 58,
        Photo = 59,
        Landscape = 60,
        Portrait = 61,
        Monochrome = 62,
    }


    public enum MultiPicture : uint
    {
        NotAvailable = 0,
        Off = 1,
        PIP = 2,
        PBPLeftRight = 3,
        PBPTopBottom = 4,
        QuadWindows = 5,
    }


    public enum Uniformity : uint
    {
        NotAvailable = 0,
        Off = 1,
        On = 2,
    }
}
