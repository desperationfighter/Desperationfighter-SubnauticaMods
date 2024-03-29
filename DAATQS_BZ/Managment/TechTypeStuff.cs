﻿using SMLHelper.V2.Handlers;


namespace DAATQS_BZ.Managment
{
    //stolen from https://github.com/PrimeSonic/PrimeSonicSubnauticaMods/blob/master/CustomCraftSML/Serialization/Components/EmTechTyped.cs#L56
    //This Function converts a String into a Techtype. This need to be done because there is a Different between Vanilla one and a Modded Item.
    public class TechTypeStuff
    {
        public static TechType GetTechType(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return TechType.None;
            }

            // Look for a known TechType
            if (TechTypeExtensions.FromString(value, out TechType tType, true))
            {
                return tType;
            }

            //  Not one of the known TechTypes - is it registered with SMLHelper?
            if (TechTypeHandler.TryGetModdedTechType(value, out TechType custom))
            {
                return custom;
            }

            return TechType.None;
        }
    }
}


