<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:Url="xrc:UrlModule"
                xmlns:Slot="xrc:SlotModule"
                exclude-result-prefixes="Url Slot">
	<xsl:output omit-xml-declaration="yes"/>

	<xsl:template match="pars">
		<h1>Welcome!</h1>

		<p>This page use the code by convention feature. This content is read from docs/loremupsum_xslt.xrc.xslt</p>

		<xsl:apply-templates select="par" />
	</xsl:template>

	<xsl:template match="par">
		<p>
			<xsl:value-of select="." />
		</p>
	</xsl:template>
</xsl:stylesheet>