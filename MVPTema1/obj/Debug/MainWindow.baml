   M S B A M L   `   `   `  ����C  ?MVPTema1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:MVPTema1MVPTema1  5   	   /clr-namespace:MVPTema1.ViewsMVPTema1.Views  5   	   9!clr-namespace:MVPTema1.ViewModelsMVPTema1.ViewModels  5   	       MVPTema1.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:MVPTema1     5   	   +vwclr-namespace:MVPTema1.Views     5   	   3vwmds!clr-namespace:MVPTema1.ViewModels     5   	     � Title$  Checkers Game��5
   	   $	��450��6   $	��720��6,   
������� � 	Resources 5       �� DataType'  !MVPTema1.ViewModels.GameViewModel   MVPTema1.Views.GameView � DataContext��%j   (��       5   1   *, +)�� 6
   modAdministratie��"  61   .R�5         6   �� 5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $��2* �6    � RowDefinitions 5   
   �� 5      $��1* �6   �� 5      $	��10* �6   .+�5   
   ]� -   5         ]�Name$ 	DockPanel��5      $y�	Collapsed=�6'   $��0��6>   $��0��6K   $��3��6[   .+�5      c� $��Stretch=�6   $��Stretch=�69   .@�5      o� .@�5      m� $
��File��6   $��0q�6-   $��0q�6A   $��6,2,7,15q�6L   .@�5      m� -   5        $��New Game��5   #   ���	#FF285C97
�������$��0q�6_   $��0,3,0,0q�6j   $��0q�6|   6�   m� 5      $
��Save��6#   ���	#FF285C97
�������$��0q�6   $��0q�6�   $��0q�6�    m� Command 61   �� *SaveGameCommand+6�   m� 5       $
��Open��6#   ���	#FF285C97
�������$��0q�6�   $��0q�6�   $��0q�6�    61   �� *OpenGameCommand+6�   m� 5!      $��
Statistics��6#   ���	#FF285C97
�������$��0q�6�   $��0q�6�   $��0q�6�    67   �� *StatisticsCommand+6�   5"      5#      o� 5$      .@�5%      m� $
��Help��6   $��0q�6-   $��0q�6A   $��6,2,7,15q�6L   .@�5&      m� $��About��6#   ���	#FF285C97
�������$��0q�6}   $��0,3,0,0q�6�   $��0q�6�    62   �� *AboutCommand+6�   5'      5(      5)      5*      �� 5+   
   $��0��6   $��1��6%   $��2��62   $��Center=�6F   .+�5,      �� $��Checkers Game��6   $��Center=�6-   $��36(�6J   $��0,0,0,20q�6X   �� 5-      -   5        $��New Game��5-      $��36(�6(   $��Center=�66   6T   5.      �� 5/   
   $��1��6   $��0��6&   $��2��66   ��6J   �� 50      51      