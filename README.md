GenericMapper
==========
<H1>What is GenericMapper ?</H1><BR/>
<b>It is a component that helps developers fix this problem brings us to map one object to another .</b><BR/><BR/>
<H1>Why use GenericMapper ?</H1><BR/>
<b>All developers agree that all we need do is apply a logic correctly, make good code and deliver a system with quality but as well we agree is that there are things that take us long but necessary , eg create DTOs (Data Transfer Objects) and to map an entity to a DTO is one of them .<BR/>
It is simple to use and is very efficient.</b><BR/><BR/>
<H1>Using GenericMapper</H1><BR/>
<b>We have a User entity with properties and one of those properties is another entity called Address</b>
public class User<BR/>
{<BR/>
public string FirstName {get; set; }<BR/>
public string LastName {get; set; }<BR/>
public Address _Address {get; set; }<BR/>
}<BR/>
<BR/>
public class Address<BR/>
{<BR/>
public string Street {get; set; }<BR/>
public string Number { get; set; }<BR/>
public string State {get; set; }<BR/>
}<BR/>
<BR/>

<b>And we are interested in a User Dto containing only first name and state .<BR/></b>
<BR/>
public class UserDto<BR/>
{<BR/>
<b>[ MapAttribute ( "FirstName")]</b><BR/>
public string Name {get; set; }<BR/>
<b>[ MapAttribute ( "_Address.State" ) ]</b><BR/>
public string State {get; set; }<BR/>
}<BR/>
<BR/>
<b>With this mapping GenericMapper may already work, and is this way.</b><BR/>
var userDto = GenericMapper.MapObject &lt;User, UserDto&gt; ( user) ;