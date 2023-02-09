row_sum = yearly.iloc[:,1:9].sum()
yearly.tail()
yearly.loc['T'] = row_sum
yearly.fillna('')
yearly.loc['T']
labels = yearly.columns
sizes = yearly.iloc[-1].plot.pie()
plt.savefig('pie_chart.png')
df = pd.read_csv('salesdaily.csv')
Datelist=df.loc[:,"datum"]
Datelist.values.tolist()
check= pd.read_csv('check.csv' ,index_col =False,header=None ,squeeze = True)
check.values.tolist()
alpha=[]
for i in range(0,len(check)):
    alpha.append(datetime.strptime(str(check[i]), '%d/%m/%Y').date())
import time as t
def wait_for_class_to_be_available(browser, total_wait=30):

    wait = WebDriverWait(browser, 500)    
    elem =wait.until(EC.presence_of_all_elements_located((By.CLASS_NAME, "mat-table.cdk-table.mat-sort.ng-star-inserted")))
    return(browser.page_source)
def indexing(html1,i):
    df1 = pd.DataFrame(columns=['date','Time','Temperature','Dew Point','Humidity','Wind','Wind Speed','Wind Gust','Pressure','Precip.','Condition'])
    soup1 = BeautifulSoup(html1,"lxml")
    table= soup1.find_all('table')
    table = soup1.find('table', class_=('mat-table cdk-table mat-sort ng-star-inserted'))
    row_marker = 0
    for row in table.find_all('tr'):
        column_marker = 0
        columns = row.find_all('td')
        if(columns != []):
            time=columns[0].text.strip()
            temperature=columns[1].text.strip()
            dew_Point=columns[2].text.strip()
            humidity=columns[3].text.strip()
            wind=columns[4].text.strip()
            wind_Speed=columns[5].text.strip()
            wind_Gust=columns[6].text.strip()
            pressure=columns[7].text.strip()
            precip=columns[8].text.strip()
            condition=columns[9].text.strip()
            df1 = df1.append({'date':i,'Time':time,'Temperature':temperature,'Dew Point':dew_Point,'Humidity':humidity,'Wind':wind,'Wind Speed':wind_Speed,'Wind Gust':wind_Gust,'Pressure':pressure,'Precip.':precip,'Condition':condition}, ignore_index=True)
    return df1
def RemoveAssci(df1,i):
    df1['Temperature']=df1['Temperature'].str.encode('ascii', 'ignore').str.decode('ascii')
    df1['Dew Point']=df1['Dew Point'].str.encode('ascii', 'ignore').str.decode('ascii')
    df1['Humidity']=df1['Humidity'].str.encode('ascii', 'ignore').str.decode('ascii')
    df1['Wind Speed']=df1['Wind Speed'].str.encode('ascii', 'ignore').str.decode('ascii')
    df1['Pressure']=df1['Pressure'].str.encode('ascii', 'ignore').str.decode('ascii')
    df1['Precip.']=df1['Precip.'].str.encode('ascii', 'ignore').str.decode('ascii')
    df1['Wind Gust']=df1['Wind Gust'].str.encode('ascii', 'ignore').str.decode('ascii')
    df1['Temperature']=df1['Temperature'].str.replace('F',' ')
    df1['Dew Point']=df1['Dew Point'].str.replace('F',' ')
    df1['Humidity']=df1['Humidity'].str.replace('%',' ')
    df1['Wind Speed']=df1['Wind Speed'].str.replace('mph',' ')
    df1['Pressure']=df1['Pressure'].str.replace('in',' ')
    df1['Precip.']=df1['Precip.'].str.replace('in',' ')
    df1['Wind Gust']=df1['Wind Gust'].str.replace('mph',' ')
    alpha.append(str(i))
    df1.to_csv('main.csv', mode='a', index=False, header=False)
    
    return (i)



driver = webdriver.Chrome('C:/chromedriver/chromedriver')
for index in range(0,len(Datelist)):
    datetime_object = datetime.strptime(str(Datelist[index]), '%m/%d/%Y').date()
#     print((alpha.count(datetime_object)),datetime_object)
#     print(datetime_object in alpha)
    if not (datetime_object in alpha):
        
        driver.get("https://www.wunderground.com/history/daily/pk/karachi/OPKC/date/"+str(datetime_object))
        html1=wait_for_class_to_be_available(driver)
        df2=indexing(html1,datetime_object)
        print(RemoveAssci(df2,datetime_object))
    else:
        index=index+1
        
        
    
