   M S B A M L   `   `   `  ����B  >1712907, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:_1712907_1712907  5   	       _1712907.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:_1712907     5   	   -   5          � Title$)  #CaroAI - Author pqviet-171207 HCMUS��5   	   $	��580��65   $	��800��6B    � Icon$ icon.png��5        � WindowStartupLocation$ CenterScreen=�5   	   ��5      �� 5   
    �� ImageSource$ bg.jpg��6   .��5      �� .+�5   
   �� -   5         ��Name$ bgRect��5       J� Opacity 6%   ��  �� ElementName$ opacitySeekBar�� �� Path$ Value� �� Mode$ OneWay=�6o   �� 5   
   -   5        $��canvas��5      $	��520��6   $	��500��6,   �� 5   
   -   5        $��
menuCanvas��5      $	��520��6#   $	��300��60   	 �� Left$		 500��6<   .+�5      �� -   5        $��
SaveButton��5      $
��SAVE��6(   $��90��6P   $��30��6[   $	 47��6g   

 �� Top$
 62��6x   $	 0.7Q�6�   �� 5      -   5        $��
LoadButton��5      $
��LOAD��6'   $��90��66   $��30��6A   $		 152��6f   $
 62��6x   $	 0.7Q�6�   �� 5      ���	#FFF4F4F5$	��121��6)   $	 31��66   
���   �$	
 379��6V   $	��232��6g   $	 0.6Q�6s   6�   �� 5      $��	Game Mode��6   $	��100��6*   $	 47��67   $	
 389��6H   $	��202��6Y   ���	#FFA410C5���	#FF094D76$
��Bold"�6�   $��16(�6�    � Cursor$
 Hand��6�   .��5      �� .+�5      	� -   5        $��	radiobox1��5   "   $��Đánh với Human��63    d� 	IsChecked$ FalseY�6L   $
 10��6^   $��Medium"�6n   
����������	#FF26CD266�   	� 5       -   5        $��	radiobox2��5    "   $��Đánh với AI��63   $
 TrueY�6I   $
 30��6Z   $��Medium"�6j   
����������	#FF1C43AC6�   5!      5"      �� 5$      $��Center=�6   $	 63��64   $	
 224��6E   .+�5%      �� $��Opacity��6   
�������$��18(�6B   6R   �� 5&      -	   5         ��Name$ opacitySeekBar��5&      $	��200��61    � Maximum$ 1Q�6=    � Minimum$ 0Q�6I    � Value$	 0.3Q�6V    �� TickPlacement$ BottomRight=�6b    �� TickFrequency$	 0.1Q�6~   5'      5)      5*      5,      