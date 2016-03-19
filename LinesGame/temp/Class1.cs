using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinesGame.temp
{
    class Class1
    {
        /*
        <Button Content="text">
            <Button.Style>
                <Style TargetType="Button">
                    <!-- Set the default value here (if any) 
                 if you set it directly on the button that will override the trigger -->
                    <Setter Property="Background" Value="LightGreen" />
                    <Style.Triggers>
                        <DataTrigger Binding="{Binding SomeConditionalProperty}"
                             Value="True">
                            <Setter Property="Background" Value="Pink" />
                        </DataTrigger>
                    </Style.Triggers>
                </Style>
            </Button.Style>
        </Button>
         */

        /*
        <!--
        <Ellipse Grid.Row="0" Grid.Column="0" Name="Cell_0_011" Stroke="{Binding Path=[0][0].color, Mode=TwoWay}" Stretch="UniformToFill">
        </Ellipse>
        
        <Button Width="50" Height="50" Command="{Binding Path=Field[0][0].color, Mode=TwoWay}" Name="Cell_0_0" Click="Cell_0_0_Click">
            <Button.Template>
                <ControlTemplate TargetType="Button">
                    <Grid>
                        <Ellipse Stroke="Black" StrokeThickness="2">
                            <Ellipse.Fill >
                                <RadialGradientBrush >
                                    <GradientStop Offset="0"
                                          Color="Azure" />
                                    <GradientStop Offset="1"
                                          Color="Lime" />
                                    <GradientStop Offset="1"
                                          Color="Gold" />
                                    <RadialGradientBrush.Transform>
                                        <TransformGroup>
                                            <ScaleTransform ScaleY="0.65" />
                                        </TransformGroup>
                                    </RadialGradientBrush.Transform>
                                </RadialGradientBrush>
                            </Ellipse.Fill>
                        </Ellipse>
                        <ContentPresenter HorizontalAlignment="Center"
                                  VerticalAlignment="Center"/>
                    </Grid>
                </ControlTemplate>
            </Button.Template>
        </Button>
        -->

        <!--
        <Button Grid.Row="0" Grid.Column="0" Content="text">
            <Button.Style>
                <Style TargetType="Button">
                    //Set the default value here (if any) if you set it directly on the button that will override the trigger 
                    <Setter Property="Background" Value="DarkGray" />
                    <Style.Triggers>
                        <DataTrigger Binding="{Binding SomeConditionalProperty}"
                             Value="True">
                            <Setter Property="Background" Value="Pink" />
                        </DataTrigger>
                    </Style.Triggers>
                </Style>
            </Button.Style>
        </Button>
        <ToggleButton Grid.Row="0" Grid.Column="4" Content="Control 1" Focusable="False" IsChecked="">
            <ToggleButton.Template>
                <ControlTemplate TargetType="{x:Type ToggleButton}">
                    <Border CornerRadius="3" Background="{TemplateBinding Background}">
                        <ContentPresenter Margin="3" HorizontalAlignment="Center" VerticalAlignment="Center"/>
                    </Border>
                    <ControlTemplate.Triggers>
                        <Trigger Property="IsChecked" Value="True">
                            <Setter Property="Background">
                                <Setter.Value>
                                    <LinearGradientBrush EndPoint="0,1" StartPoint="0,0">
                                        <GradientStop Color="#FFF3F3F3" Offset="1"/>
                                        <GradientStop Color="LawnGreen" Offset="0.307"/>
                                    </LinearGradientBrush>
                                </Setter.Value>
                            </Setter>
                        </Trigger>
                        <Trigger Property="IsChecked" Value="False">
                            <Setter Property="Background">
                                <Setter.Value>
                                    <LinearGradientBrush EndPoint="0,1" StartPoint="0,0">
                                        <GradientStop Color="#FFF3F3F3" Offset="1"/>
                                        <GradientStop Color="{Binding Path=Field, Mode=TwoWay, Converter={converters:CellToColorConverter}}" Offset="0.307"/>
                                    </LinearGradientBrush>
                                </Setter.Value>
                            </Setter>
                        </Trigger>
                    </ControlTemplate.Triggers>
                </ControlTemplate>
            </ToggleButton.Template>
        </ToggleButton>
        -->
         */

        /*
         <Button.Style>
                <Style TargetType="Button">
                    <Setter Property="OverridesDefaultStyle" Value="True"/>
                    <Setter Property="Margin" Value="5"/>
                    <Setter Property="Background" Value="{Binding Path=Field[0], Mode=TwoWay, Converter={converters:CellToColorConverter}, ConverterParameter='0'}"/>
                    <Setter Property="BorderBrush" Value="{Binding Path=Field[0].Selected, Mode=TwoWay, Converter={converters:CellSelectToColor}}"/>
                    <Setter Property="BorderThickness" Value="2"/>
                    <Setter Property="Template">
                        <Setter.Value>
                            <ControlTemplate TargetType="Button">
                                <Border Name="border" CornerRadius="20" Background="{TemplateBinding Background}" BorderBrush="{TemplateBinding BorderBrush}" BorderThickness="{TemplateBinding BorderThickness}">
                                    <ContentPresenter HorizontalAlignment="Center" VerticalAlignment="Center" />
                                </Border>
                                <ControlTemplate.Triggers>
                                    <Trigger Property="IsPressed" Value="True">
                                        <Setter Property="Background" TargetName="border" Value="{Binding Path=Field[0], Mode=TwoWay, Converter={converters:CellToColorConverter}, ConverterParameter='0'}"/>
                                        <Setter Property="BorderBrush" TargetName="border" Value="GreenYellow"/>
                                    </Trigger>
                                    

                                </ControlTemplate.Triggers>
                            </ControlTemplate>
                        </Setter.Value>
                    </Setter>
                </Style>
            </Button.Style> 
            <!--<Button.Template>
                <ControlTemplate TargetType="Button">
                    <Grid Background="{Binding Path=Field[0], Mode=TwoWay, Converter={converters:CellToColorConverter}, ConverterParameter='0'}">
                        <Ellipse Stroke="Black" StrokeThickness="1">
                            <Ellipse.Fill >
                                <RadialGradientBrush >
                                    <GradientStop Offset="0"
                                          Color="Azure" />

                                    <GradientStop Offset="1"
                                          Color="SaddleBrown" />
                                    <RadialGradientBrush.Transform>
                                        <TransformGroup>
                                            <ScaleTransform ScaleY="0.65" />
                                        </TransformGroup>
                                    </RadialGradientBrush.Transform>
                                </RadialGradientBrush>

                            </Ellipse.Fill>
                        </Ellipse>
                        <ContentPresenter HorizontalAlignment="Center"
                                  VerticalAlignment="Center"/>
                    </Grid>
                </ControlTemplate>
            </Button.Template>       -->
        */

        /*
        <Style x:Key="FolderTemplate" TargetType="{x:Type Button}">
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate  TargetType="Button">
                        <Grid>
                            <Border Name="ButtonBorder" CornerRadius="7,7,7,7"  
                    BorderBrush="Black" BorderThickness="2">
                                <ContentPresenter/>
                            </Border>
                        </Grid>
                        <ControlTemplate.Triggers>
                            <Trigger Property="Button.IsPressed" Value="True">
                                <Setter TargetName="ButtonBorder" Property="BorderBrush"
                                        Value="Orange" />
                            </Trigger>
                        </ControlTemplate.Triggers>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>

        <Style TargetType="Button" x:Key="ButtonStyle">
            <Setter Property="OverridesDefaultStyle" Value="True"/>
            <Setter Property="Margin" Value="5"/>
            <Setter Property="Background" Value="White"/>
            <Setter Property="BorderBrush" Value="#2b4e9f"/>
            <Setter Property="BorderThickness" Value="2"/>
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="Button">
                        <Border Name="border" CornerRadius="20" Background="{TemplateBinding Background}" BorderBrush="{TemplateBinding BorderBrush}" BorderThickness="{TemplateBinding BorderThickness}">
                            <ContentPresenter HorizontalAlignment="Center" VerticalAlignment="Center" />
                        </Border>
                        <ControlTemplate.Triggers>
                            <Trigger Property="IsPressed" Value="True">
                                <Setter Property="Background" TargetName="border" Value="Green"/>
                                <Setter Property="BorderBrush" TargetName="border" Value="GreenYellow"/>
                            </Trigger>
                        </ControlTemplate.Triggers>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>

        
        */

        /*
         <!--
        <Button Content="Click Me" Background="Blue">
    <Button.Template>
        <ControlTemplate TargetType="{x:Type Button}">
            <Border Name="Border" BorderBrush="{TemplateBinding BorderBrush}" BorderThickness="{TemplateBinding BorderThickness}" Background="{TemplateBinding Background}">
                <ContentPresenter Content="{TemplateBinding Content}" ContentTemplate="{TemplateBinding ContentTemplate}" Margin="{TemplateBinding Padding}" />
            </Border>
            <ControlTemplate.Triggers>
                <Trigger Property="Button.IsFocused" Value="True">
                    <Setter TargetName="Border" Property="Background" Value="White" />
                </Trigger>
            </ControlTemplate.Triggers>
        </ControlTemplate>
    </Button.Template>
</Button>
        -->
        */

        /*
         <ListView Grid.Row="0" Width="100" Height="400" Grid.Column="10" Name="ListViewEmployeeDetails" Margin="4,109,12,23"  ItemsSource="{Binding Path=Field}"  >
            <ListView.View>
                <GridView x:Name="grdTest">
                    <GridViewColumn Header="ID"  Width="100">
                        <GridViewColumn.CellTemplate>
                            <DataTemplate>
                                <TextBlock x:Name="idField" Text="{Binding Selected}" TextDecorations="Underline" Foreground="Blue"/>
                                <DataTemplate.Triggers>
                                    <DataTrigger Binding="{Binding Selected}">
                                        <DataTrigger.EnterActions>
                                            <BeginStoryboard>
                                                <Storyboard>
                                                    <DoubleAnimation Duration="0:0:5" To="0.0" Storyboard.TargetProperty="Opacity" Storyboard.TargetName="idField"/>
                                                </Storyboard>
                                            </BeginStoryboard>
                                        </DataTrigger.EnterActions>
                                    </DataTrigger>
                                </DataTemplate.Triggers>
                            </DataTemplate>
                        </GridViewColumn.CellTemplate>
                    </GridViewColumn>
                    <GridViewColumn Header="Name" DisplayMemberBinding="{Binding CellColor}"  Width="100" />
                    <GridViewColumn Header="Price" DisplayMemberBinding="{Binding ContainBall}" Width="100" />
                    <GridViewColumn Header="Reliab" DisplayMemberBinding="{Binding Reliability}" Width="100" />
                </GridView>
            </ListView.View>
        </ListView>
        */
    }
}
