   M S B A M L   `   `   `  ����C  ?MVPTema1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:MVPTema1.ModelsMVPTema1.Models  5          MVPTema1.Views.GameView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0modelsclr-namespace:MVPTema1.Models     5      ���	#FF8B79C5  � 	Resources  5	         
BoardStyle �� 
TargetType �� Triggers ^� 
Conditions   MVPTema1.Models.CellColor   Brown �� Value �� Binding �� Property �� Value
	   Tan
   	CadetBlue��%�  &	        �� 5
   
   " ��6#    5      ^� 5       5      �� 5      # Z 6=    6$   �� *Cell+6=   0�5      �� 5       � ��
 �**��6>   5      ^� 5       5      �� 5      # Z	 6=    6$   �� *Cell+6=   0�5      �� 5       � ��
 �����6>   5      ^� 5       5      �� 5      # Z
 6=    6$   �� *Cell+6=   0�5       �� 5!       � ��
 ���_�6>   5#      5%      5&      .��5'      �  � ColumnDefinitions 5(   
   �� 5)      $��2* �6   �� 5*      $��2* �6   �� 5+      $��* �6   .+�5.   
   �� -   5         ��Name$ Board��5.      $��0��6D   $��2��6T   ��6&   �� *Board+��5/      �� 50      .R�51      �� #��[  6   $��0q�52      ��53      ��  �� ElementName$ Board�� �� Path$$ DataContext.CellClickedCommand���54      ��  J� IsHitTestVisible 55      �� *IsHitTestVisible+��57      �� 58      ��6$   �� *
Resource+5:      5;      ��5=      �� 5>      .�5?      ��  � �� 5@      �� *	CellPos.Y+�� 5A       � �� 5B      �� *	CellPos.X+5C      ��5E      �� 5F      .R�5G      �  5H      �� 5I      61   �� 5J      61   �� 5K      61   �� 5L      61   �� 5M      61   �� 5N      61   �� 5O      61   �� 5P      61    � RowDefinitions 5R      �� 5S      6.   �� 5T      6.   �� 5U      6.   �� 5V      6.   �� 5W      6.   �� 5X      6.   �� 5Y      6.   �� 5Z      6.   5\      5]      5_      � 5a   
   $��2��6    5b      �� 5c       5e      �� 5f      $��2* �6    �� 5g      $��* �6    �� 5h      $��* �6    �� 5i      $��3* �6    �� 5j      $��3* �6    .+�5m      �� $��1��6   $��Current Player��6%   
�������$��Center=�6N   $��Center=�6k   $��20(�6�   �� 5n      $��2��6   
�������$��Center=�6W   $��Center=�6t   $��20(�6�   ��6%   �� *CurrentPlayer+6�   �� 5o      $��4��6   
�������$��Center=�6P   $��Center=�6m   $��20(�6�   ��6%   �� *Winner+6�   5p      5q      5r      