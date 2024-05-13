# BSC5-parser
Simple parser for the [Yale's Bright Star Catalog 5](http://tdc-www.harvard.edu/catalogs/bsc5.html) written in C#


## Header
The first 28 bytes of both <i>BSC5</i> and <i>BSC5ra</i> contain the
following information:
```
Integer*4 STAR0=0	Subtract from star number to get sequence number
Integer*4 STAR1=1	First star number in file
Integer*4 STARN=9110  	Number of stars in file
Integer*4 STNUM=1	0 if no star i.d. numbers are present
			1 if star i.d. numbers are in catalog file
			2 if star i.d. numbers are <region><nnnn> in file
Logical*4 MPROP=1	1 if proper motion is included
			0 if no proper motion is included
Integer*4 NMAG=-1	Number of magnitudes present (-1=J2000 instead of B1950)
Integer*4 NBENT=32	Number of bytes per star entry
```

## Entry
Each catalog entry in <i>BSC5</i> and <i>BSC5ra</i> contains 32 bytes with
the following information:
```
Real*4 XNO		Catalog number of star
Real*8 SRA0		B1950 Right Ascension (radians)
Real*8 SDEC0		B1950 Declination (radians)
Character*2 IS		Spectral type (2 characters)
Integer*2 MAG		V Magnitude * 100
Real*4 XRPM		R.A. proper motion (radians per year)
Real*4 XDPM		Dec. proper motion (radians per year)
```