   M S B A M L   `   `   `  ���� D  @LinesGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:LinesGame.PropertiesLinesGame.Properties  ;"clr-namespace:LinesGame.ConvertorsLinesGame.Convertors  "    LinesGame.View.GameSetupView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     0p"clr-namespace:LinesGame.Properties     9
converters"clr-namespace:LinesGame.Convertors       � Title$&   Выбор режима игры��$	��250��$	��280�� � 
ResizeMode$ NoResize=� � 	Resources 0   *LinesGame.Convertors.GameTypeEnumConvertor     KeyGameTypeEnumConvertor  .��� $��0,0,4,0q�.+��� $��Режим игры��$
��Left=�$��	10,10,0,0q�$	��Top=�$��77��$	��243��.��� $��4,9,-2,0q�.+�	�  	� 	GroupName$ 	fieldSize��$)��#Ограниченное время��$
��Left=�$	��Top=�$��10,0,0,0q� d� 	IsChecked �� #  LinesGame.Properties.Settings   Default �� Source# Z  �� Path$ GameType� �� Mode$ TwoWay=�	 �� 	Converter	   
 �� ConverterParameter$
 LimitedTime��	� $ 	fieldSize��$>��8Ограниченное количество ходов��$
��Left=�$	��Top=�$��	10,21,0,0q� �� # Z $ GameType�$ TwoWay=�	   $
 LimitedMoves���� $'��!Количество цветов��$
��Left=�$��	10,92,0,0q�$	��Top=�$��77��$	��243��.��� .+��� -    ��Name$ colorCounter��$
��Left=�$��28��$	��Top=�$	��171��$��	50,10,0,0q� � Maximum$ 7Q� � Minimum$ 4Q� �� IsSnapToTickEnabled .  �� TickFrequency$ 1Q� � Value �� # Z $ 
ColorCount�$ TwoWay=��� $
��Left=�$��8,10,0,0q�$	��Top=�$��31������  �� ElementName$ colorCounter��$ Value��� $��22,32,186,290q��� -    ��Name$ startBtn��$��
Старт��$
��Left=�$��
10,174,0,0q�$	��Top=�$	��100��$��28���� -   $ 	cancelBtn��$��Отмена��$
��Left=�$��153,174,0,0q�$	��Top=�$	��100��$��28��