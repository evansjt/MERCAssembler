=====================================================================

Welcome to the MERC Assembler - Version 1.0 Instruction Guide
         ___           ___           ___           ___     
        /\__\         /\  \         /\  \         /\  \    
       /::|  |       /::\  \       /::\  \       /::\  \   
      /:|:|  |      /:/\:\  \     /:/\:\  \     /:/\:\  \  
     /:/|:|__|__   /::\~\:\  \   /::\~\:\  \   /:/  \:\  \ 
    /:/ |::::\__\ /:/\:\ \:\__\ /:/\:\ \:\__\ /:/__/ \:\__\
    \/__/~~/:/  / \:\~\:\ \/__/ \/_|::\/:/  / \:\  \  \/__/
          /:/  /   \:\ \:\__\      |:|::/  /   \:\  \      
         /:/  /     \:\ \/__/      |:|\/__/     \:\  \     
        /:/  /       \:\__\        |:|  |        \:\__\    
        \/__/         \/__/         \|__|         \/__/

Copyright (c) - Nick Carpenter, Jake Evans, Craig McGee, Angel Rivera

=====================================================================

How to run:
-----------

To run the MERC Assembler application, open the file named "MERC.exe". This file will only work for Windows machines.


---------------------------------------------------------------------


How to operate:
---------------

Here are all possible commands to use for this console application:

- "run [filename] [arg0] [arg1]" : runs the entire .merc file*
- "debug [filename] [arg0] [arg1]" : steps through each instruction of the.merc file*
	=> "n" : moves on to next instruction
	=> "q" : quits debug mode
	=> "r" : prints registers at current state
	=> "d" : prints data segment at current state
	=> "s" : prints stack at current state
	=> "a" : prints registers, data segment, and stack at current state
- "write [filename]" : writes machine code from input into a .coe file in the "output" directory
- "link [filename1] [filename2] ..." : links multiple .merc programs and writes their machine code sequentially into a .coe file in the "output" directory
- "help" : prints all possible commands for this console application
- "exit" : exits the console application

*(if arg0 or arg1 are not provided, they are automatically set to zero)

** NOTE: [filename] must be a .merc file (named with no spaces) inside the "input" directory. **

All example .merc files are in the "input" folder in the current directory. If you would like to test a custom .merc file, you can save it in the "input" directory so that the assembler can identify it by name when prompted.

=====================================

Have fun!

=====================================
