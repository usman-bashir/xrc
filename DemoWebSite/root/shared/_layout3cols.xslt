<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:Slot="xrc:SlotModule" 
                exclude-result-prefixes="Slot">
	<xsl:output method="html" encoding="utf-8" />
	<xsl:param name="title" />

	<xsl:template match="/">
    <h1>
      <xsl:value-of select="$title"/>
    </h1>

    <div>
      <div style="float:left;width:30%">
        <xsl:value-of disable-output-escaping="yes" select="Slot:Include('left')" />
      </div>
      <div style="float:left;width:40%">
        <xsl:value-of disable-output-escaping="yes" select="Slot:Include('center')" />
      </div>
      <div style="float:left;width:30%">
        <xsl:value-of disable-output-escaping="yes" select="Slot:Include('right')" />
      </div>
      <div style="clear:both">
      </div>
    </div>

  </xsl:template>
</xsl:stylesheet>