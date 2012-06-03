<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:param name="genre"/>
  
  <xsl:template match="bookstore">
    <HTML>
      <BODY>

        <h1>Books by genre <xsl:value-of select="$genre"/>
        </h1>
        
        <TABLE BORDER="2">
          <TR>
            <TD>ISBN</TD>
            <TD>Title</TD>
            <TD>Price</TD>
          </TR>
          <xsl:apply-templates select="book[@genre=$genre]"/>
        </TABLE>
      </BODY>
    </HTML>
  </xsl:template>
  <xsl:template match="book">
    <TR>
      <TD>
        <xsl:value-of select="@ISBN"/>
      </TD>
      <TD>
        <xsl:value-of select="title"/>
      </TD>
      <TD>
        <xsl:value-of select="price"/>
      </TD>
    </TR>
  </xsl:template>
</xsl:stylesheet>