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

    <div class="row-fluid">
      <div class="span4">
        <xsl:value-of disable-output-escaping="yes" select="Slot:IncludeChild('left')" />
      </div>
      <div class="span4">
        <xsl:value-of disable-output-escaping="yes" select="Slot:IncludeChild('center')" />
      </div>
      <div class="span4">
        <xsl:value-of disable-output-escaping="yes" select="Slot:IncludeChild('right')" />
      </div>
    </div>

  </xsl:template>
</xsl:stylesheet>