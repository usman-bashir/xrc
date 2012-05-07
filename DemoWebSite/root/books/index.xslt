<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:Url="xrc:UrlModule"
                xmlns:Html="xrc:HtmlModule" 
                exclude-result-prefixes="Html Url">
  <xsl:output omit-xml-declaration="yes"/>
  
  <xsl:template match="bookstore">
    <h1>Books</h1>

    <TABLE BORDER="2">
      <TR>
        <TD>ISBN</TD>
        <TD>Title</TD>
        <TD>Price</TD>
        <TD>Genre</TD>
      </TR>
      <xsl:apply-templates select="book"/>
    </TABLE>
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
      <TD>
        <a href="{Url:Content('~/books/genre', @genre)}">
          <xsl:value-of select="@genre"/>
        </a>
      </TD>
    </TR>
  </xsl:template>
</xsl:stylesheet>