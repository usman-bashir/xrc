<xsl:stylesheet version="1.0"
                xmlns="http://www.w3.org/1999/xhtml"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:Url="xrc:UrlModule"
                xmlns:Html="xrc:HtmlModule" 
                xmlns:Slot="xrc:SlotModule" 
                exclude-result-prefixes="Html Url Slot">
	<xsl:output method="html" encoding="utf-8" />
	<xsl:param name="title" />

	<xsl:template match="/">
    <xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
    <html>
      <head>
        <title>
          <xsl:value-of select="$title" />
        </title>
        <link rel="stylesheet" type="text/css" href="{Url:Content('~/css/demo.css')}" />
      </head>
      <body>

        <xsl:value-of disable-output-escaping="yes" select="Html:Action('~/shared/_header')" />

        <xsl:value-of disable-output-escaping="yes" select="Slot:Include()" />

        <xsl:value-of disable-output-escaping="yes" select="Html:Action('~/shared/_footer')" />

      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>