   M S B A M L   `   `   `  ���� B  >1712907, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:_1712907_1712907      _1712907.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:_1712907     -     � Title$)  #CaroAI - Author pqviet-171207 HCMUS��$	��580��$	��800�� � Icon$ icon.png�� � WindowStartupLocation$ CenterScreen=�����  �� ImageSource$ bg.jpg��.���� .+��� -    ��Name$ bgRect�� J� Opacity ��  �� ElementName$ opacitySeekBar�� �� Path$ Value� �� Mode$ OneWay=��� -   $��canvas��$	��520��$	��500���� -   $��
menuCanvas��$	��520��$	��300��	 �� Left$		 500��.+��� -   $��
SaveButton��$
��SAVE��$��90��$��30��$	 47��

 �� Top$
 62��$	 0.7Q��� -   $��
LoadButton��$
��LOAD��$��90��$��30��$		 152��$
 62��$	 0.7Q��� ���	#FFF4F4F5$	��121��$	 31��
���   �$	
 379��$	��232��$	 0.6Q��� $��	Game Mode��$	��100��$	 47��$	
 389��$	��202�����	#FFA410C5���	#FF094D76$
��Bold"�$��16(� � Cursor$
 Hand��.���� .+�	� -   $��	radiobox1��$��Đánh với Human�� d� 	IsChecked$ FalseY�$
 10��$��Medium"�
����������	#FF26CD26	� -   $��	radiobox2��$��Đánh với AI��$
 TrueY�$
 30��$��Medium"�
����������	#FF1C43AC�� $��Center=�$	 63��$	
 224��.+��� $��Opacity��
�������$��18(��� -	    ��Name$ opacitySeekBar��$	��200�� � Maximum$ 1Q� � Minimum$ 0Q� � Value$	 0.3Q� �� TickPlacement$ BottomRight=� �� TickFrequency$	 0.1Q�