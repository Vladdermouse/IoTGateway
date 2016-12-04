﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waher.Persistence.Files.Serialization.ValueTypes
{
	public class EnumSerializer : ValueTypeSerializer
	{
		private Type enumType;
		private bool asInt;

		public EnumSerializer(Type EnumType)
		{
			this.enumType = EnumType;
			this.asInt = this.enumType.IsDefined(typeof(FlagsAttribute), false);
		}

		public override Type ValueType
		{
			get
			{
				return this.enumType;
			}
		}

		public override object Deserialize(BinaryDeserializer Reader, uint? DataType, bool Embedded)
		{
			if (!DataType.HasValue)
				DataType = Reader.ReadBits(6);

			switch (DataType.Value)
			{
				case ObjectSerializer.TYPE_BOOLEAN: return Enum.ToObject(this.enumType, Reader.ReadBoolean() ? 1 : 0);
				case ObjectSerializer.TYPE_BYTE: return Enum.ToObject(this.enumType, Reader.ReadByte());
				case ObjectSerializer.TYPE_INT16: return Enum.ToObject(this.enumType, Reader.ReadInt16());
				case ObjectSerializer.TYPE_INT32: return Enum.ToObject(this.enumType, Reader.ReadInt32());
				case ObjectSerializer.TYPE_INT64: return Enum.ToObject(this.enumType, Reader.ReadInt64());
				case ObjectSerializer.TYPE_SBYTE: return Enum.ToObject(this.enumType, Reader.ReadSByte());
				case ObjectSerializer.TYPE_UINT16: return Enum.ToObject(this.enumType, Reader.ReadUInt16());
				case ObjectSerializer.TYPE_UINT32: return Enum.ToObject(this.enumType, Reader.ReadUInt32());
				case ObjectSerializer.TYPE_UINT64: return Enum.ToObject(this.enumType, Reader.ReadUInt64());
				case ObjectSerializer.TYPE_DECIMAL: return Enum.ToObject(this.enumType, Reader.ReadDecimal());
				case ObjectSerializer.TYPE_DOUBLE: return Enum.ToObject(this.enumType, Reader.ReadDouble());
				case ObjectSerializer.TYPE_SINGLE: return Enum.ToObject(this.enumType, Reader.ReadSingle());
				case ObjectSerializer.TYPE_STRING: return Enum.Parse(this.enumType, Reader.ReadString());
				case ObjectSerializer.TYPE_ENUM: return Reader.ReadEnum(this.enumType);
				case ObjectSerializer.TYPE_NULL: return null;
				default: throw new Exception("Expected an enum value.");
			}
		}

		public override void Serialize(BinarySerializer Writer, bool WriteTypeCode, bool Embedded, object Value)
		{
			if (WriteTypeCode)
			{
				if (this.asInt)
					Writer.WriteBits(ObjectSerializer.TYPE_INT32, 6);
				else
					Writer.WriteBits(ObjectSerializer.TYPE_ENUM, 6);
			}

			if (this.asInt)
				Writer.Write((int)Value);
			else
				Writer.Write((Enum)Value);
		}

	}
}
