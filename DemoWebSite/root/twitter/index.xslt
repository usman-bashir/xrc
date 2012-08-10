<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:google="http://base.google.com/ns/1.0">
	<xsl:output omit-xml-declaration="yes"/>

	<xsl:param name="hashTag"/>

	<xsl:template match="rss/channel">

		<form>
			<input name="hashTag" value="{$hashTag}" />
			<input type="submit" />
		</form>

		<h1>
			Searching for <xsl:value-of select="$hashTag" />
		</h1>

		<table class="table table-striped">
			<thead>
				<tr>
					<th>title</th>
					<th>pubDate</th>
					<th>Image</th>
				</tr>
			</thead>
			<tbody>
				<xsl:apply-templates select="item"/>
			</tbody>
		</table>
	</xsl:template>

	<xsl:template match="item">
		<TR>
			<TD>
				<a href="{link}">
					<xsl:value-of select="title"/>
				</a>
			</TD>
			<TD>
				<xsl:value-of select="pubDate"/>
			</TD>
			<TD>
				<img src="{google:image_link}" />
			</TD>
		</TR>
	</xsl:template>
</xsl:stylesheet>