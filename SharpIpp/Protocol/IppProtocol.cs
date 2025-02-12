using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SharpIpp.Exceptions;
using SharpIpp.Protocol.Extensions;
using SharpIpp.Protocol.Models;

namespace SharpIpp.Protocol
{
    /// <summary>
    ///     Ipp protocol reader-writer.
    ///     Ipp protocol only supports common types:
    ///     <see cref="int"/>
    ///     <see cref="bool"/>
    ///     <see cref="string" />
    ///     <see cref="DateTimeOffset" />
    ///     <see cref="NoValue" />
    ///     <see cref="Range" />
    ///     <see cref="Resolution" />
    ///     <see cref="StringWithLanguage" />
    ///     all other types must be mapped via IMapper in-onto these
    /// </summary>
    public partial class IppProtocol : IIppProtocol
    {
        public async Task WriteIppRequestAsync(IIppRequestMessage ippRequestMessage, Stream stream, CancellationToken cancellationToken = default)
        {
            if (ippRequestMessage is null)
                throw new ArgumentNullException( nameof( ippRequestMessage ) );
            if (stream is null)
                throw new ArgumentNullException( nameof( stream ) );
            using var writer = new BinaryWriter(stream, Encoding.ASCII, true);
            writer.WriteBigEndian( ippRequestMessage.Version.ToInt16BigEndian() );
            writer.WriteBigEndian( (short)ippRequestMessage.IppOperation );
            writer.WriteBigEndian( ippRequestMessage.RequestId );
            WriteSections(ippRequestMessage, writer);
            if (ippRequestMessage.Document != null)
            {
                await ippRequestMessage.Document.CopyToAsync(stream, 81920, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<IIppRequestMessage> ReadIppRequestAsync( Stream stream, CancellationToken cancellationToken = default )
        {
            if (stream is null)
                new ArgumentException( nameof( stream ) );
            using var reader = new BinaryReader( stream, Encoding.ASCII, true );
            return await ReadIppRequestAsync( reader, cancellationToken );
        }

        public Task<IIppResponseMessage> ReadIppResponseAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream is null)
                new ArgumentException( nameof( stream ) );
            var res = new IppResponseMessage();
            try
            {
                using var reader = new BinaryReader(stream, Encoding.ASCII, true);
                res.Version = new IppVersion( reader.ReadInt16BigEndian() );
                res.StatusCode = (IppStatusCode)reader.ReadInt16BigEndian();
                res.RequestId = reader.ReadInt32BigEndian();
                ReadSections(reader, res);
                return Task.FromResult((IIppResponseMessage)res);
            }
            catch (Exception ex)
            {
                throw new IppResponseException($"Failed to parse ipp response. Current response parsing ended on: \n{res}", ex, res);
            }
        }

        private void ReadSections(BinaryReader reader, IIppResponseMessage res)
        {
            IppAttribute? prevAttribute = null;
            List<IppAttribute>? attributes = null;

            do
            {
                var data = reader.ReadByte();
                var sectionTag = (SectionTag)data;

                switch (sectionTag)
                {
                    //https://tools.ietf.org/html/rfc8010#section-3.5.1
                    case SectionTag.EndOfAttributesTag: return;
                    case SectionTag.Reserved:
                    case SectionTag.OperationAttributesTag:
                    case SectionTag.JobAttributesTag:
                    case SectionTag.PrinterAttributesTag:
                    case SectionTag.UnsupportedAttributesTag:
                    case SectionTag.SubscriptionAttributesTag:
                    case SectionTag.EventNotificationAttributesTag:
                    case SectionTag.ResourceAttributesTag:
                    case SectionTag.DocumentAttributesTag:
                    case SectionTag.SystemAttributesTag:
                        var section = new IppSection { Tag = sectionTag };
                        res.Sections.Add(section);
                        attributes = section.Attributes;
                        break;
                    default:
                        if (attributes is null)
                            throw new ArgumentException( $"Section start tag not found in stream. Expected < 0x06. Actual: {data}" );
                        var attribute = ReadAttribute((Tag)data, reader, prevAttribute);
                        prevAttribute = attribute;
                        attributes.Add(attribute);
                        break;
                }
            }
            while (true);
        }

        private void ReadSections( BinaryReader reader, IIppRequestMessage res )
        {
            IppAttribute? prevAttribute = null;
            List<IppAttribute>? attributes = null;
            do
            {
                var data = reader.ReadByte();
                var sectionTag = (SectionTag)data;

                switch ( sectionTag )
                {
                    case SectionTag.OperationAttributesTag:
                        attributes = res.OperationAttributes;
                        break;
                    case SectionTag.JobAttributesTag:
                        attributes = res.JobAttributes;
                        break;
                    case SectionTag.PrinterAttributesTag:
                        attributes = res.PrinterAttributes;
                        break;
                    case SectionTag.SubscriptionAttributesTag:
                        attributes = res.SubscriptionAttributes;
                        break;
                    case SectionTag.EventNotificationAttributesTag:
                        attributes = res.EventNotificationAttributes;
                        break;
                    case SectionTag.ResourceAttributesTag:
                        attributes = res.ResourceAttributes;
                        break;
                    case SectionTag.DocumentAttributesTag:
                        attributes = res.DocumentAttributes;
                        break;
                    case SectionTag.SystemAttributesTag:
                        attributes = res.SystemAttributes;
                        break;
                    case SectionTag.EndOfAttributesTag:
                        return;
                    default:
                        if ( attributes is null )
                        {
                            reader.BaseStream.Position--;
                            return;
                        }
                        var attribute = ReadAttribute( (Tag)data, reader, prevAttribute );
                        prevAttribute = attribute;
                        attributes.Add( attribute );
                        break;
                }
            }
            while ( true );
        }

        public void WriteAttribute( BinaryWriter stream, IppAttribute attribute, IppAttribute? prevAttribute )
        {
            stream.Write( (byte)attribute.Tag );

            if (prevAttribute != null && prevAttribute.Name == attribute.Name)
            {
                stream.WriteBigEndian( (short)0 );
            }
            else
            {
                stream.WriteBigEndian( (short)attribute.Name.Length );
                stream.Write( Encoding.ASCII.GetBytes( attribute.Name ) );
            }

            var value = attribute.Value;
            WriteValue( value, stream );
        }

        public void WriteValue(object value, BinaryWriter stream)
        {
            //https://tools.ietf.org/html/rfc8010#section-3.5.2
            switch (value)
            {
                case NoValue v:
                    Write(v, stream);
                    break;
                case int v:
                    Write(v, stream);
                    break;
                case bool v:
                    Write(v, stream);
                    break;
                case string v:
                    Write(v, stream);
                    break;
                case DateTimeOffset v:
                    Write(v, stream);
                    break;
                case Resolution v:
                    Write(v, stream);
                    break;
                case Range v:
                    Write(v, stream);
                    break;
                case StringWithLanguage v:
                    Write(v, stream);
                    break;
                default:
                    throw new ArgumentException($"Type {value.GetType()} not supported in ipp");
            }
        }

        public Task WriteIppResponseAsync( IIppResponseMessage ippResponseMessage, Stream stream, CancellationToken cancellationToken = default )
        {
            if (ippResponseMessage is null)
                throw new ArgumentNullException( nameof( ippResponseMessage ) );
            if (stream is null)
                throw new ArgumentNullException( nameof( stream ) );
            using var writer = new BinaryWriter( stream, Encoding.ASCII, true );
            writer.WriteBigEndian( ippResponseMessage.Version.ToInt16BigEndian() );
            writer.WriteBigEndian( (short)ippResponseMessage.StatusCode );
            writer.WriteBigEndian( ippResponseMessage.RequestId );
            WriteSections( ippResponseMessage, writer );
            return Task.CompletedTask;
        }

        public object ReadValue(BinaryReader stream, Tag tag)
        {
            if (stream is null)
                throw new ArgumentNullException( nameof( stream ) );
            //https://tools.ietf.org/html/rfc8010#section-3.5.2
            return tag switch
            {
                Tag.Unsupported => ReadNoValue(stream),
                Tag.Unknown => ReadNoValue(stream),
                Tag.NoValue => ReadNoValue(stream),
                Tag.Integer => ReadInt(stream),
                Tag.Enum => ReadInt(stream),
                Tag.Boolean => ReadBool(stream),
                Tag.OctetStringWithAnUnspecifiedFormat => ReadString(stream),
                Tag.DateTime => ReadDateTimeOffset(stream),
                Tag.Resolution => ReadResolution(stream),
                Tag.RangeOfInteger => ReadRange(stream),
                Tag.BegCollection => ReadString(stream), // BegCollection has an optional string value.
                Tag.TextWithLanguage => ReadStringWithLanguage(stream),
                Tag.NameWithLanguage => ReadStringWithLanguage(stream),
                Tag.EndCollection => ReadString(stream), // EndCollection has an optional string value.
                Tag.TextWithoutLanguage => ReadString(stream),
                Tag.NameWithoutLanguage => ReadString(stream),
                Tag.Keyword => ReadString(stream),
                Tag.Uri => ReadString(stream),
                Tag.UriScheme => ReadString(stream),
                Tag.Charset => ReadString(stream),
                Tag.NaturalLanguage => ReadString(stream),
                Tag.MimeMediaType => ReadString(stream),
                Tag.MemberAttrName => ReadString(stream), // MemberNameAttr has no name but it's value is the name for the following attributes.
                Tag.OctetStringUnassigned38 => ReadString(stream),
                Tag.OctetStringUnassigned39 => ReadString(stream),
                Tag.OctetStringUnassigned3A => ReadString(stream),
                Tag.OctetStringUnassigned3B => ReadString(stream),
                Tag.OctetStringUnassigned3C => ReadString(stream),
                Tag.OctetStringUnassigned3D => ReadString(stream),
                Tag.OctetStringUnassigned3E => ReadString(stream),
                Tag.OctetStringUnassigned3F => ReadString(stream),
                Tag.IntegerUnassigned20 => ReadInt(stream),
                Tag.IntegerUnassigned24 => ReadInt(stream),
                Tag.IntegerUnassigned25 => ReadInt(stream),
                Tag.IntegerUnassigned26 => ReadInt(stream),
                Tag.IntegerUnassigned27 => ReadInt(stream),
                Tag.IntegerUnassigned28 => ReadInt(stream),
                Tag.IntegerUnassigned29 => ReadInt(stream),
                Tag.IntegerUnassigned2A => ReadInt(stream),
                Tag.IntegerUnassigned2B => ReadInt(stream),
                Tag.IntegerUnassigned2C => ReadInt(stream),
                Tag.IntegerUnassigned2D => ReadInt(stream),
                Tag.IntegerUnassigned2E => ReadInt(stream),
                Tag.IntegerUnassigned2F => ReadInt(stream),
                Tag.StringUnassigned40 => ReadString(stream),
                Tag.StringUnassigned43 => ReadString(stream),
                Tag.StringUnassigned4B => ReadString(stream),
                Tag.StringUnassigned4C => ReadString(stream),
                Tag.StringUnassigned4D => ReadString(stream),
                Tag.StringUnassigned4E => ReadString(stream),
                Tag.StringUnassigned4F => ReadString(stream),
                Tag.StringUnassigned50 => ReadString(stream),
                Tag.StringUnassigned51 => ReadString(stream),
                Tag.StringUnassigned52 => ReadString(stream),
                Tag.StringUnassigned53 => ReadString(stream),
                Tag.StringUnassigned54 => ReadString(stream),
                Tag.StringUnassigned55 => ReadString(stream),
                Tag.StringUnassigned56 => ReadString(stream),
                Tag.StringUnassigned57 => ReadString(stream),
                Tag.StringUnassigned58 => ReadString(stream),
                Tag.StringUnassigned59 => ReadString(stream),
                Tag.StringUnassigned5A => ReadString(stream),
                Tag.StringUnassigned5B => ReadString(stream),
                Tag.StringUnassigned5C => ReadString(stream),
                Tag.StringUnassigned5D => ReadString(stream),
                Tag.StringUnassigned5E => ReadString(stream),
                Tag.StringUnassigned5F => ReadString(stream),
                _ => throw new ArgumentException($"Ipp tag {tag} not supported")
            };
        }

        public IppCollection ReadCollection(BinaryReader stream, string name)
        {
            var begValue = ReadValue(stream, Tag.BegCollection);
            var attributes = new List<IppAttribute>();
            IppAttribute? prevAttribute = null;
            var tag = (Tag)stream.ReadByte();
            while (true)
            {
                prevAttribute = ReadAttribute(tag, stream, prevAttribute);
                if (prevAttribute.Tag == Tag.EndCollection)
                {
                    break;
                }

                attributes.Add(prevAttribute);
                tag = (Tag)stream.ReadByte();
            }

            return new IppCollection(new(Tag.BegCollection, name, begValue), prevAttribute, attributes);
        }

        public IppAttribute ReadAttribute(Tag tag, BinaryReader stream, IppAttribute? prevAttribute)
        {
            if (stream is null)
                throw new ArgumentNullException( nameof( stream ) );
            var len = stream.ReadInt16BigEndian();
            var name = Encoding.ASCII.GetString(stream.ReadBytes(len));
            var normalizedName = GetNormalizedName(tag, name, prevAttribute);

            var value = tag == Tag.BegCollection ? ReadCollection(stream, normalizedName) : ReadValue(stream, tag);
            var attribute = new IppAttribute(tag, normalizedName, value);
            return attribute;
        }

        private string GetNormalizedName(Tag tag, string name, IppAttribute? prevAttribute)
        {
            if (!string.IsNullOrEmpty(name))
                return name;
            
            if (tag == Tag.EndCollection || tag == Tag.MemberAttrName)
            {
                // EndCollection can optionally have a name. That would be handled above.
                return string.Empty;
            }

            if (prevAttribute?.Name is string previousName && !string.IsNullOrEmpty(previousName))
            {
                return previousName;
            }

            if (prevAttribute is not null
                && prevAttribute.Tag == Tag.MemberAttrName
                && prevAttribute.Value is string nameFromMemberAttrName
                && !string.IsNullOrEmpty(nameFromMemberAttrName))
            {
                return nameFromMemberAttrName;
            }

            throw new ArgumentException("0 length attribute name found not in a 1setOf");
        }

        private async Task<IIppRequestMessage> ReadIppRequestAsync( BinaryReader reader, CancellationToken cancellationToken = default )
        {
            IppRequestMessage message = new IppRequestMessage
            {
                Version = new IppVersion( reader.ReadInt16BigEndian() ),
                IppOperation = (IppOperation)reader.ReadInt16BigEndian(),
                RequestId = reader.ReadInt32BigEndian()
            };
            ReadSections( reader, message );
            message.Document = new MemoryStream();
            await reader.BaseStream.CopyToAsync( message.Document );
            message.Document.Seek( 0, SeekOrigin.Begin );
            return message;
        }

        private void WriteSections(IIppRequestMessage requestMessage, BinaryWriter writer)
        {
            if (requestMessage.OperationAttributes.Count == 0
                && requestMessage.JobAttributes.Count == 0
                && requestMessage.PrinterAttributes.Count == 0
                && requestMessage.UnsupportedAttributes.Count == 0
                && requestMessage.SubscriptionAttributes.Count == 0
                && requestMessage.EventNotificationAttributes.Count == 0
                && requestMessage.ResourceAttributes.Count == 0
                && requestMessage.DocumentAttributes.Count == 0
                && requestMessage.SystemAttributes.Count == 0)
                return;
            WriteSection(SectionTag.OperationAttributesTag, requestMessage.OperationAttributes, writer);
            WriteSection(SectionTag.JobAttributesTag, requestMessage.JobAttributes, writer);
            WriteSection(SectionTag.PrinterAttributesTag, requestMessage.PrinterAttributes, writer);
            WriteSection(SectionTag.UnsupportedAttributesTag, requestMessage.UnsupportedAttributes, writer);
            WriteSection(SectionTag.SubscriptionAttributesTag, requestMessage.SubscriptionAttributes, writer);
            WriteSection(SectionTag.EventNotificationAttributesTag, requestMessage.EventNotificationAttributes, writer);
            WriteSection(SectionTag.ResourceAttributesTag, requestMessage.ResourceAttributes, writer);
            WriteSection(SectionTag.DocumentAttributesTag, requestMessage.DocumentAttributes, writer);
            WriteSection(SectionTag.SystemAttributesTag, requestMessage.SystemAttributes, writer);
            //end-of-attributes-tag https://tools.ietf.org/html/rfc8010#section-3.5.1
            writer.Write((byte)SectionTag.EndOfAttributesTag);
        }

        private void WriteSections( IIppResponseMessage responseMessage, BinaryWriter writer )
        {
            foreach (var section in responseMessage.Sections)
                WriteSection( section.Tag, section.Attributes, writer );
            //end-of-attributes-tag https://tools.ietf.org/html/rfc8010#section-3.5.1
            writer.Write( (byte)SectionTag.EndOfAttributesTag );
        }

        public void WriteSection( SectionTag sectionTag, List<IppAttribute> attributes, BinaryWriter writer )
        {
            if (attributes is null)
                throw new ArgumentNullException( nameof( attributes ) );
            if (writer is null)
                throw new ArgumentNullException( nameof( writer ) );
            if (attributes.Count == 0)
                return;
            //operation-attributes-tag https://tools.ietf.org/html/rfc8010#section-3.5.1
            writer.Write( (byte)sectionTag );
            for (var i = 0; i < attributes.Count; i++)
                WriteAttribute( writer, attributes[ i ], i > 0 ? attributes[ i - 1 ] : null );
        }
    }
}
