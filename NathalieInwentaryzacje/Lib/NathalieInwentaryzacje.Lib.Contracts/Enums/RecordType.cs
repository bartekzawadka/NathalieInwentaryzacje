using System.ComponentModel;

namespace NathalieInwentaryzacje.Lib.Contracts.Enums
{
    public enum RecordType
    {
        [Description("ZŁOTO")]
        Gold,

        [Description("SREBRO")]
        Silver,

        [Description("BIŻUTERIA SZTUCZNA")]
        ArtificialJewellery,

        [Description("WYROBY WŁASNE - ZŁOTO")]
        CustomGoldenProducts,

        [Description("OPAKOWANIA")]
        Wrappings,

        [Description("PLATERY")]
        Platers
    }
}
