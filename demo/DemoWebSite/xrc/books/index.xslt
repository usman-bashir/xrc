<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:Url="xrc:UrlModule"
                xmlns:Slot="xrc:SlotModule"
                exclude-result-prefixes="Url Slot">
	<xsl:output omit-xml-declaration="yes"/>

	<xsl:template match="bookstore">
		<h1>Books</h1>

		<table class="table table-striped">
			<thead>
				<TR>
					<th>ISBN</th>
					<th>Title</th>
					<th>Price</th>
					<th>Genre</th>
				</TR>
			</thead>
			<tbody>
				<xsl:apply-templates select="book"/>
			</tbody>
		</table>

		<div>
			<xsl:value-of select="Slot:Include('_loremIpsum')" disable-output-escaping="yes" />
		</div>
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
				<a href="{Url:Content('genre', @genre)}">
					<xsl:value-of select="@genre"/>
				</a>
			</TD>
		</TR>
	</xsl:template>
</xsl:stylesheet>