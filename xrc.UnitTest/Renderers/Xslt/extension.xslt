<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
                xmlns:MyModule="xrc:MyModuleExt">
  <xsl:output method="text"/>
  <xsl:template match="/">
    <xsl:value-of select="MyModule:SayHelloFromModule()" />
  </xsl:template>
</xsl:stylesheet>