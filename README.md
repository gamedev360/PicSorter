**Word Count**<br/>
https://github.com/LuminosoInsight/code-sample-term-counting<br/><br/>

**Description**<br/>
A word counting application for a Luminoso job application test.<br/><br/>

This application requires a text file as input and will produce the complete count of words used in the text file.<br/><br/>

**Version of Python Used**<br/>
Python 3.6.3 :: Anaconda, Inc.<br/><br/>

**Running**<br/>
python word_count [-hj] [\<inputfile><br/\>]
- -h help<br/>
- -j output in JSON<br/>

if the optional \<inputfile\> is not specified, then stdin will be read<br/><br/>

**GIT**<br/>
The application build includes a git repository of checkins as the application was being built. Use 'git log' to see the history.<br/><br/>

**Design Considerations**<br/>
I considered making the application object oriented, but the application being more of a one off bash script style application the introduction of object code vs. simple functions seemed over kill. It could have gone either way. I chose simple functions. Consider it an early design choice.<br/><br/>


**Enhancements**<br/>
I would consider the following enhancements
1. A file containing a list of non-words
1. A file containing a list of words that were similar and should be counted together
1. A file containing a list of words that are too common to count and maybe a option to toggle on/off

