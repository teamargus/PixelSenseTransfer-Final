   M S B A M L   `   `   `  ����  tagVisualizationTest    demoSoftware.MainWindow   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      e aMicrosoft.Surface.Presentation, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e351s)http://schemas.microsoft.com/surface/2008  5      J FWpfAnimatedGif, Version=1.4.12.0, Culture=neutral, PublicKeyToken=null,gif"http://wpfanimatedgif.codeplex.com  5        � Title$  Demo Software��5      .��5	      � -   5        $��myGrid��5	      $
��1080��6   $
��1920��6*    �� IsFlicksEnabled .  .+�5   
   ;  5Microsoft.Surface.Presentation.Controls.TagVisualizer  -   5        $��myTag��5         Definitions 5      H  BMicrosoft.Surface.Presentation.Controls.TagVisualizationDefinition  5         Value<  6Microsoft.Surface.Presentation.Input.TagValueConverter$ 0 6/      Source$ TagVisualization.xaml?�5         UsesTagOrientation .    TagRemovedBehavior$ 	Disappear=�5      5      �� 5   
   -   5        $��
loadingImg��5      "  WpfAnimatedGif.ImageBehavior   RepeatBehavior$ Forever��5         AnimatedSource$ C:\loading5.gif��5      $	��300��5       $	��300��6   $��
0, 0, 0, 0q�6)   �� 5"   
   -   5        $��button0��5"   \   $��Update��6   $��23��6"   $
��Left=�6.   $��	0,514,0,0q�6I   $	��Top=�6k   $��75��6�   6�   �� 5$      -   5        $��textBox��5%   
   $	��100��5&      $	��400��5'      $��Right=�5(      $	��Top=�5)      	 �� TextWrapping$
	 Wrap=�5*   
   
 �� AcceptsReturn
 . " �� VerticalScrollBarVisibility$ Visible=�5,   
   .��5-   
   5.      �� 50   
   -   5        $��canvas��50      .+�52      �� -   5         ��Name$ sPanel��52      $	��338��6)   $	��100��66   .+�53      �� -   5        $
��lbl1��53      $��24��6$   $��24��6/   $��24��6;   $��0, 0, 0, 15q�6J   �� 54      -	   5        $
��lbl2��54      $��24��6$   $��24��6/   $��24��6;   $��0, 0, 0, 15q�6J   �� 55      -
   5        $
��lbl3��55      $��24��6$   $��24��6/   $��24��6;   $��0, 0, 0, 15q�6J   �� 56      -   5        $
��lbl4��56      $��24��6$   $��24��6/   $��24��6;   $��0, 0, 0, 15q�6J   �� 57      -   5        $
��lbl5��57      $��24��6$   $��24��6/   $��24��6;   $��0, 0, 0, 15q�6J   �� 58      -   5        $
��lbl6��58      $��24��6$   $��24��6/   $��24��6;   $��0, 0, 0, 15q�6J   �� 59      -   5        $
��lbl7��59      $��24��6$   $��24��6/   $��24��6;   $��0, 0, 0, 15q�6J   �� 5:      -   5        $
��lbl8��5:      $��24��6$   $��24��6/   $��24��6;   $��0, 0, 0, 15q�6J   5;      5<      5>      5?      