using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SpectroWizard.data
{
    public abstract class DataTable<HeaderType,RowHeaderType,ColumnHeaderType,MainType>
    {
        HeaderType HeaderData;
        List<RowHeaderType> RowsData = new List<RowHeaderType>();
        List<RowHeaderType> RowsComment = new List<RowHeaderType>();
        List<ColumnHeaderType> ColumnData = new List<ColumnHeaderType>();
        List<List<MainType>> Data = new List<List<MainType>>();

        public DataTable()
        {
        }

        protected abstract HeaderType LoadHeader(BinaryReader br);
        protected abstract ColumnHeaderType LoadCol(BinaryReader br);
        protected abstract RowHeaderType LoadRow(BinaryReader br);
        protected abstract MainType LoadData(BinaryReader br);

        protected abstract void SaveHeader(BinaryWriter bw,HeaderType data);
        protected abstract void SaveCol(BinaryWriter bw,ColumnHeaderType data);
        protected abstract void SaveRow(BinaryWriter bw,RowHeaderType data);
        protected abstract void SaveData(BinaryWriter bw,MainType data);

        public HeaderType Header
        {
            get
            {
                return HeaderData;
            }
            set
            {
                HeaderData = value;
            }
        }

        public MainType this[int c, int r]
        {
            get
            {
                return Data[r][c];
            }
            set
            {
                Data[r][c] = value;
            }
        }

        public int GetRowCount()
        {
            return RowsData.Count;
        }

        public RowHeaderType GetRow(int index)
        {
            return RowsData[index];
        }

        public int GetColumnCount()
        {
            return ColumnData.Count;
        }

        public RowHeaderType GetRowHeader(int row)
        {
            return RowsData[row];
        }

        public void SetRowHeader(int row, RowHeaderType data)
        {
            RowsData[row] = data;
        }

        public int InsetRowInto(int index,RowHeaderType header, MainType default_val)
        {
            int ret = index;
            RowsData.Insert(index,header);
            Data.Insert(index,new List<MainType>());
            for (int c = 0; c < ColumnData.Count; c++)
                Data[ret].Add(default_val);
            return ret;
        }

        public int AddRow(RowHeaderType header,MainType default_val)
        {
            int ret = RowsData.Count;
            RowsData.Add(header);
            Data.Add(new List<MainType>());
            for (int c = 0; c < ColumnData.Count; c++)
                Data[ret].Add(default_val);
            return ret;
        }

        public void RemoveRow(int index)
        {
            RowsData.RemoveAt(index);
            Data.RemoveAt(index);
        }

        public ColumnHeaderType GetColHeader(int col)
        {
            return ColumnData[col];
        }

        public void SetColHeader(int col, ColumnHeaderType data)
        {
            ColumnData[col] = data;
        }

        public int InsertCol(int index,ColumnHeaderType header, MainType defaul_val)
        {
            int ret = index;// ColumnData.Count;
            ColumnData.Insert(index,header);// .Add(header);
            for (int r = 0; r < RowsData.Count; r++)
                Data[r].Insert(index,defaul_val);//.Add(defaul_val);
            return ret;
        }

        public int AddCol(ColumnHeaderType header, MainType defaul_val)
        {
            int ret = ColumnData.Count;
            ColumnData.Add(header);
            for (int r = 0; r < RowsData.Count; r++)
                Data[r].Add(defaul_val);
            return ret;
        }

        public void RemoveElement(int index)
        {
            ColumnData.RemoveAt(index);
            for (int r = 0; r < RowsData.Count; r++)
                Data[r].RemoveAt(index);
        }

        public void Load(BinaryReader br)
        {
            byte ver = br.ReadByte();
            if (ver != 1)
                throw new Exception("Неподдерживаемая версия DataTable. Файл повреждён или от более старшей версии.");

            HeaderData = LoadHeader(br);
            
            int cc = br.ReadInt32();
            int rc = br.ReadInt32();

            RowsData.Clear();
            ColumnData.Clear();
            Data.Clear();

            for (int c = 0; c < cc; c++)
                ColumnData.Add(LoadCol(br)); //SaveCol(bw, ColumnData[c]);

            for (int r = 0; r < rc; r++)
            {
                RowsData.Add(LoadRow(br)); //SaveRow(bw, RowsData[r]);
                Data.Add(new List<MainType>());
                for (int c = 0; c < cc; c++)
                    Data[r].Add(LoadData(br)); //SaveData(bw, DataTable[r][c]);
            }

            ver = br.ReadByte();
            if (ver != 231)
                throw new Exception("Неверная концовка DataTable. Файл повреждён.");
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write((byte)1);
            SaveHeader(bw,HeaderData);
            bw.Write(ColumnData.Count);
            bw.Write(RowsData.Count);
            
            for (int c = 0; c < ColumnData.Count; c++)
                SaveCol(bw, ColumnData[c]);

            for (int r = 0; r < RowsData.Count; r++)
            {
                SaveRow(bw,RowsData[r]);
                for (int c = 0; c < ColumnData.Count; c++)
                    SaveData(bw,Data[r][c]);
            }
            bw.Write((byte)231);
        }
    }
}
