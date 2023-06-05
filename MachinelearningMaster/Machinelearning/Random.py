import pandas as pd
import warnings
import pandas as pd
from sklearn.model_selection import GridSearchCV, cross_val_score, train_test_split
from sklearn.preprocessing import StandardScaler
from sklearn.linear_model import LogisticRegression
from sklearn.tree import DecisionTreeClassifier
from sklearn.ensemble import RandomForestClassifier, GradientBoostingClassifier
from sklearn.svm import SVC

# disable all warnings
warnings.filterwarnings('ignore')
pd.set_option('display.max_columns', None)

# Load your dataset into a pandas DataFrame
df = pd.read_csv('DailyDataset.csv')

# Define the date ranges for each season across all six years
spring_ranges = [pd.date_range(start=f'{year}-03-01', end=f'{year}-05-31') for year in range(2014, 2020)]
summer_ranges = [pd.date_range(start=f'{year}-06-01', end=f'{year}-08-31') for year in range(2014, 2020)]
fall_ranges = [pd.date_range(start=f'{year}-09-01', end=f'{year}-11-30') for year in range(2014, 2020)]
winter_ranges = [pd.date_range(start=f'{year}-12-01', end=f'{year + 1}-02-28') for year in range(2014, 2020)]

# Define the temperature ranges and corresponding values for each part of the season
cold_range = (float('-inf'), 59)
cool_range = (59, 77)
warm_range = (77, 95)
hot_range = (95, float('inf'))


df['season'] = ''
# Loop through each row in the DataFrame
for i, row in df.iterrows():
    # Check which season the row's date falls under
    if row['date'] in spring_ranges[0] or row['date'] in spring_ranges[1] or row['date'] in spring_ranges[2] \
        or row['date'] in spring_ranges[3] or row['date'] in spring_ranges[4] or row['date'] in spring_ranges[5]:
        season = 'spring'
    elif row['date'] in summer_ranges[0] or row['date'] in summer_ranges[1] or row['date'] in summer_ranges[2] \
        or row['date'] in summer_ranges[3] or row['date'] in summer_ranges[4] or row['date'] in summer_ranges[5]:
        season = 'summer'
    elif row['date'] in fall_ranges[0] or row['date'] in fall_ranges[1] or row['date'] in fall_ranges[2] \
        or row['date'] in fall_ranges[3] or row['date'] in fall_ranges[4] or row['date'] in fall_ranges[5]:
        season = 'fall'
    else:
        season = 'winter'
    df.at[i, 'season'] = season
    # Determine which part of the season the row's temperature falls under
    temp = row['Temperature']
    if temp < cold_range[1]:
        part = 'early'
    elif cool_range[0] <= temp < cool_range[1]:
        part = 'mid'
    elif warm_range[0] <= temp < warm_range[1]:
        part = 'late'
    else:
        part = 'end'
        
        # Assign the appropriate value to the row based on the part and season
    if part == 'early':
        if season == 'spring':
            df.loc[i, 'R06'] = row['R06'] + 5
        elif season == 'summer':
            df.loc[i, 'R06'] = row['R06'] + 8
        elif season == 'fall':
            df.loc[i, 'R06'] = row['R06'] + 11
        elif season == 'winter':
            df.loc[i, 'R06'] = row['R06']
    elif part == 'mid':
        if season == 'spring':
            df.loc[i, 'R06'] = row['R06'] + 5
        elif season == 'summer':
            df.loc[i, 'R06'] = row['R06'] + 8
        elif season == 'fall':
            df.loc[i, 'R06'] = row['R06'] + 11
        elif season == 'winter':
            df.loc[i, 'R06'] = row['R06']
    elif part == 'late':
        if season == 'spring':
            df.loc[i, 'R06'] = row['R06'] + 5
        elif season == 'summer':
            df.loc[i, 'R06'] = row['R06'] + 8
        elif season == 'fall':
            df.loc[i, 'R06'] = row['R06'] + 11
        elif season == 'winter':
            df.loc[i, 'R06'] = row['R06']
    else:
        if season == 'spring':
            df.loc[i, 'R06'] = row['R06'] + 1
        elif season == 'summer':
            df.loc[i, 'R06'] = row['R06'] + 2
        elif season == 'fall':
            df.loc[i, 'R06'] = row['R06'] -2
        elif season == 'winter':
            df.loc[i, 'R06'] = row['R06']
   

print(df)


import pandas as pd

# Use qcut to classify "R06" column into three equal frequency bins
df['Ro6Classification'] = pd.qcut(df['R06'], q=3, labels=[0, 1,2])


print(df)

X= df[['Year','Month','Day','Temperature','Dew Point','Humidity','Wind','Wind Speed','Pressure','IntCondition']]
y = df["Ro6Classification"]

# Split the data into training and testing sets
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=42)

# Standardize the features using StandardScaler
scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

# Define the classifiers to be tested and their hyperparameter grids
classifiers = {
    "Logistic Regression": (LogisticRegression(max_iter=5000), 
                            {'C': [0.01, 0.1, 1.0, 10, 100], 'penalty': ['l1', 'l2', 'elasticnet', 'none'],
                             'solver': ['newton-cg', 'lbfgs', 'liblinear', 'sag', 'saga'],
                             'class_weight': ['balanced', None]}),
    "Decision Tree": (DecisionTreeClassifier(), 
                      {'criterion': ['gini', 'entropy'], 'max_depth': [3, 5,10,15,20, None],
                       'min_samples_split': [2, 5, 10], 'min_samples_leaf': [1, 2, 4]}),
    "Random Forest": (RandomForestClassifier(), 
                      {'n_estimators': [50, 100, 150,200,250,300], 'max_depth': [3, 5,10,15, None],
                       'max_features': ['sqrt', 'log2'], 'min_samples_split': [2, 5, 10],
                       'min_samples_leaf': [1, 2, 4], 'bootstrap': [True, False]}),
    "SVM": (SVC(), 
            {'C': [0.1, 1, 10,15,20,30,50,100], 'kernel': ['linear', 'rbf'], 'gamma': ['scale', 'auto'],
             'class_weight': ['balanced', None]}),
    "Gradient Boosting": (GradientBoostingClassifier(), 
                          {'n_estimators': [50, 100, 200,600], 'max_depth': [3, 15,30,50, None],
                           'max_features': ['sqrt', 'log2'], 'learning_rate': [0.01, 0.1, 1],
                           'subsample': [0.5, 0.8, 1.0]})
}
from sklearn.model_selection import RandomizedSearchCV

# Perform cross-validation and hyperparameter tuning for each classifier
results = {}
print("Starting the loop over classifiers...")
for clf_name, (clf, param_grid) in classifiers.items():
    print("Running RandomizedSearchCV for classifier:", clf_name)
    random_search = RandomizedSearchCV(clf, param_distributions=param_grid, n_iter=50, cv=5)
    random_search.fit(X_train_scaled, y_train)
    best_clf = random_search.best_estimator_
    accuracy_scores = cross_val_score(best_clf, X_train_scaled, y_train, cv=10)
    results[clf_name] = {'Best Params': best_clf.get_params(), 
                         'Mean Accuracy': accuracy_scores.mean(),
                         'Accuracy STD': accuracy_scores.std()}

print("Printing the final results DataFrame...")
results_df = pd.DataFrame.from_dict(results, orient='index')
print(results_df)
