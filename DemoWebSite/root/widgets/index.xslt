<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:Url="urn:xrc/Url"
                xmlns:Html="urn:xrc/Html" 
                exclude-result-prefixes="Html Url">
  <xsl:output omit-xml-declaration="yes"/>
  
  <xsl:template match="/">
    <h1>Widgets</h1>

		<div>
			<div>
				Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
			</div>
			<div style="float:left">
				<xsl:value-of disable-output-escaping="yes" select="Html:Page('~/widgets/_ranking')"/>
				<xsl:text> </xsl:text>
			</div>
			<div style="float:left">
				<xsl:value-of disable-output-escaping="yes" select="Html:Page('~/widgets/_latestphotos')"/>
				<xsl:text> </xsl:text>
			</div>
			<div style="clear:both">
				<xsl:text> </xsl:text>
			</div>
		</div>

	</xsl:template>
  
</xsl:stylesheet>