using MudBlazor;

namespace StudentManagementBlazor.Themes;

public static class AppTheme
{
    public static MudTheme Default = new MudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#CD0000",
            Background = "#EFEDE6",
            Surface = "#FFFFFF",

            TextPrimary = "#1F1F1F",
            TextSecondary = "#666666",

            AppbarBackground = "#CD0000",
            AppbarText = "#FFFFFF",

            DrawerBackground = "#FFFFFF",
            DrawerText = "#1F1F1F",

            Divider = "#D8D5CC"
        },

        PaletteDark = new PaletteDark()
        {
            Primary = "#FF4D4D",
            Background = "#121212",
            Surface = "#1E1E1E",

            TextPrimary = "#EFEDE6",
            TextSecondary = "#BDBDBD",

            AppbarBackground = "#1E1E1E",
            AppbarText = "#EFEDE6",

            DrawerBackground = "#181818",
            DrawerText = "#EFEDE6",

            Divider = "#333333"
        }
    };
}