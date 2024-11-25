const Reports = () => ({ language = 'th' }) => {
    const data = [
      { name: 'A', value: 400 },
      { name: 'B', value: 300 },
      { name: 'C', value: 300 },
      { name: 'D', value: 200 },
    ];
  
    const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042'];
  
    const translations = {
      th: {
        title: 'รายงาน',
        analytics: 'การวิเคราะห์',
        distribution: 'การกระจายข้อมูล'
      },
      en: {
        title: 'Reports',
        analytics: 'Analytics',
        distribution: 'Data Distribution'
      }
    };
  
    const currentLanguage = translations[language] ? language : 'th';
    const t = translations[currentLanguage];
  
    return (
        <div className="space-y-6">
        <h2 className="text-lg font-bold">{t.title}</h2>
      </div>
    );
  };
  
  export default Reports;