<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

  <xsl:output method="text"/>
  
  <xsl:template match="bookstore">
    <xsl:apply-templates select="book" />
  </xsl:template>
  <xsl:template match="book">
    <xsl:value-of select="@ISBN"/>
    <xsl:text>|</xsl:text>
    <xsl:if test="author/last-name">
      <xsl:value-of select="author/last-name"/>
    </xsl:if>
    <xsl:if test="author/name">
      <xsl:value-of select="author/name"/>
    </xsl:if>
    <xsl:if test="position()!=last()">
      <xsl:text>,</xsl:text>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>