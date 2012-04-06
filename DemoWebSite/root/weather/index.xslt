<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output omit-xml-declaration="yes"/>

  <xsl:param name="location"/>

  <xsl:template match="weather">

    <xsl:param name="description" select="forecast_information/city/@data" />
    
    <form>
      <input name="location" value="{$location}" />
      <input type="submit" />
    </form>
    
    <h1>Weather of <xsl:value-of select="$description" /></h1>

    <table border="2">
      <tr>
        <td>Day</td>
        <td>Condition</td>
        <td>T°</td>
      </tr>
      <xsl:apply-templates select="forecast_conditions"/>
    </table>
  </xsl:template>
  
  <xsl:template match="forecast_conditions">
    <TR>
      <TD>
        <xsl:value-of select="day_of_week/@data"/>
      </TD>
      <TD>
        <xsl:value-of select="condition/@data"/>
        <img src="http://www.google.com/{icon/@data}" />
      </TD>
      <TD>
        <xsl:value-of select="low/@data"/>
        -
        <xsl:value-of select="high/@data"/>
      </TD>
    </TR>
  </xsl:template>
</xsl:stylesheet>