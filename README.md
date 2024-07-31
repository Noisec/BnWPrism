# BnWPrism
An antivirus 'simulator' that makes your PC look scary to viruses.

As you may know, most of the latest viruses check for running debuggers, antiviruses, and whether the machine is a VM or not.
Derby simulates these processes (or at least 91 of them) that can scare away viruses. When it is launched, it only uses 33.3MB of RAM.
- Tested on random trojans and stealers found on the net, and it is working (most of the viruses stop execution after checking for debuggers).

If you want to use it, just put its shortcut into shell:startup and check the hidden launch option

any.run test:

before:

  ![x](https://github.com/Noisec/pic-s/blob/main/images/derby-bef.png?raw=true)

after:

  ![x](https://github.com/Noisec/pic-s/blob/main/images/derby-aft.png?raw=true)

<sup>(no, i did not terminate the process)</sup>


<sup>(the derby.exe that gets cloned and launched with different names is found at https://github.com/Noisec/small_exe)</sup>

<sup>old icon: Flaticon.com</sup>

<sup>(Yes, I have renamed it to BnWPrism)</sup>
